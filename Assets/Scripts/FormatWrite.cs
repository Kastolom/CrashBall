using System;

namespace Assets.Scripts
{  
    static class FormatWrite
    {
        const float THOUSAND = 1000f;
        const float MILLION = 1000000f;
        const float BILLION = 1000000000f;

        static public string FormatNumber(float _money, int realCount)
        {
            if (_money > BILLION)
            {
                _money = (float)Math.Round(_money / BILLION, realCount);
                return _money.ToString() + "b";
            } else
            if (_money > MILLION)
            {
                _money = (float)Math.Round(_money / MILLION, realCount);
                return _money.ToString() + "m";
            }
            else
            if (_money > THOUSAND)
            {
                _money = (float)Math.Round(_money / THOUSAND, realCount);
                return _money.ToString() + "k";
            }
            else
            {
                _money = (float)Math.Round(_money, realCount);
                return _money.ToString();
            }
        }
    }
}
