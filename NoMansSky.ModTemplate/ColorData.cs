using libMBIN.NMS;
using Reloaded.ModHelper;

namespace NoMansSky.ModTemplate
{
    class ColorData
    {
        const float colorChangeAmount = 0.003f;
        private bool rCountingDown;
        private bool gCountingDown;
        private bool bCountingDown;

        private float previousR;
        private float previousG;
        private float previousB;

        private bool isInit = false;

        public ColorData()
        {

        }

        public ColorData(float r, float g, float b)
        {
            previousR = r;
            previousG = g;
            previousB = b;
            isInit = true;
        }

        public Colour ModifyColor(Colour colorToModify)
        {
            if (!isInit)
            {
                previousR = colorToModify.R;
                previousG = colorToModify.G;
                previousB = colorToModify.B;
                isInit = true;
            }

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

            return colorToModify;
        }
    }
}
