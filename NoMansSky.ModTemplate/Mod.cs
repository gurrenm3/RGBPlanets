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
        int framesBetweenUpdates = 3;
        int frameCount = 0;

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

        /// <summary>
        /// Initializes your mod along with some necessary info.
        /// </summary>
        public Mod(IModConfig _config, IReloadedHooks _hooks, IModLogger _logger) : base(_config, _hooks, _logger)
        {
            
        }

        /// <summary>
        /// Called once every frame.
        /// </summary>
        public async override void Update()
        {
            if (!Game.IsInGame)
                return;

            frameCount++;
            if (frameCount <= framesBetweenUpdates)
                return;

            await Game.Colors.DaySkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                {
                    ModifyColorSetting(setting);
                }
            });

            await Game.Colors.DuskSkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                {
                    ModifyColorSetting(setting);
                }
            });

            await Game.Colors.NightSkyColors.ModifyAsync<GcWeatherColourSettings>(colorSettings =>
            {
                foreach (var setting in colorSettings.GenericSettings.Settings)
                {
                    ModifyColorSetting(setting);
                }
            });

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

            colorData = new ColorData(colour.R, colour.G, colour.B);
            colorDataByName.Add(settingName, colorData);
            return colorData;
        }
    }
}