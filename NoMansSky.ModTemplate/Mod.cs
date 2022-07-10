using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.ModHelper;
using NoMansSky.Api;
using libMBIN.NMS;
using libMBIN.NMS.Globals;
using libMBIN.NMS.GameComponents;
using System.Collections.Generic;

namespace NoMansSky.ModTemplate
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : NMSMod
    {
        /// <summary>
        /// Initializes your mod along with some necessary info.
        /// </summary>
        public Mod(IModConfig _config, IReloadedHooks _hooks, IModLogger _logger) : base(_config, _hooks, _logger)
        {

        }

        /// <summary>
        /// Contains the name of each color variable and it's associated color data.
        /// </summary>
        Dictionary<string, ColorData> colorDataByName = new Dictionary<string, ColorData>()
        {
            { "CloudColour1", new ColorData() },
            { "CloudColour2", new ColorData() },
            { "FogColour", new ColorData() },
            { "HeightFogColour", new ColorData() },
            { "HorizonColour", new ColorData() },
            { "LightColour", new ColorData() },
            { "SkyColour", new ColorData() },
            { "SkySolarColour", new ColorData() },
            { "SkyUpperColour", new ColorData() },
            { "SunColour", new ColorData() }
        };

        int framesBetweenUpdates = 3;
        int frameCount = 0;

        /// <summary>
        /// Called once every frame.
        /// </summary>
        public async override void Update()
        {
            // don't run this unless we're in game.
            if (!Game.IsInGame)
                return;

            // don't run this every frame. Give it a little time for the next color change so it looks smooth.
            frameCount++;
            if (frameCount <= framesBetweenUpdates)
                return;

            // Modify day colors. Use async version so it doesn't lock game.
            await Game.Colors.DaySkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                    ModifyColorSetting(setting);
            });

            // Modify dusk colors. Use async version so it doesn't lock game.
            await Game.Colors.DuskSkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                    ModifyColorSetting(setting);
            });

            // Modify night colors. Use async version so it doesn't lock game.
            await Game.Colors.NightSkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                    ModifyColorSetting(setting);
            });

            // reset frame count
            frameCount = 0;
        }

        /// <summary>
        /// Handles the logic for modifying each color.
        /// </summary>
        /// <param name="colorSetting"></param>
        private void ModifyColorSetting(GcPlanetWeatherColourData colorSetting)
        {
            foreach (var color in colorSetting.GetAllColours())
            {
                var data = GetColorData(color.Key, color.Value);
                if (data == null)
                    continue;

                var result = data.ModifyColor(color.Value);
                color.Value.R = result.R;
                color.Value.G = result.G;
                color.Value.B = result.B;
            }
        }

        /// <summary>
        /// Returns color data for the color with the provided setting name.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="colour"></param>
        /// <returns></returns>
        private ColorData GetColorData(string settingName, Colour colour)
        {
            if (colorDataByName.TryGetValue(settingName, out var colorData))
                return colorData;

            colorData = new ColorData();
            colorDataByName.Add(settingName, colorData);
            return colorData;
        }
    }
}