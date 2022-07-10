using libMBIN.NMS;
using Reloaded.ModHelper;

namespace NoMansSky.ModTemplate
{
    /// <summary>
    /// This class is used to track colors and apply the actual changes to them.
    /// </summary>
    class ColorData
    {
        const float colorChangeAmount = 0.003f;

        // these variables track whether or not this color is counting down from 1 to 0.
        private bool rCountingDown;
        private bool gCountingDown;
        private bool bCountingDown;

        public ColorData()
        {

        }

        /// <summary>
        /// Actually modifies the color, adjusting it slightly in the opposite direction.
        /// </summary>
        /// <param name="colorToModify"></param>
        /// <returns></returns>
        public Colour ModifyColor(Colour colorToModify)
        {
            // adjust R values
            colorToModify.R += rCountingDown ? -colorChangeAmount : colorChangeAmount;
            if (colorToModify.R <= 0f)
            {
                colorToModify.R = 0f;
                rCountingDown = false;
            }
            else if (colorToModify.R >= 1f)
            {
                colorToModify.R = 1f;
                rCountingDown = true;
            }


            // adjust G values
            colorToModify.G += gCountingDown ? -colorChangeAmount : colorChangeAmount;
            if (colorToModify.G <= 0f)
            {
                colorToModify.G = 0f;
                gCountingDown = false;
            }
            else if (colorToModify.G >= 1f)
            {
                colorToModify.G = 1f;
                gCountingDown = true;
            }


            // adjust B values
            colorToModify.B += bCountingDown ? -colorChangeAmount : colorChangeAmount;
            if (colorToModify.B <= 0f)
            {
                colorToModify.B = 0f;
                bCountingDown = false;
            }
            else if (colorToModify.B >= 1f)
            {
                colorToModify.B = 1f;
                bCountingDown = true;
            }


            // return modified color
            return colorToModify;
        }
    }
}
