using System;

namespace Assets.Scripts
{
    static class FormatWrite
    {
        static public string FormatNumber(float _money, int realCount)
        {
            if (_money > 1000000f)
            {
                _money = (float)Math.Round(_money / 1000000, realCount);
                return _money.ToString() + "m";
            }
            else
            if (_money > 1000f)
            {
                _money = (float)Math.Round(_money / 1000, realCount);
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
