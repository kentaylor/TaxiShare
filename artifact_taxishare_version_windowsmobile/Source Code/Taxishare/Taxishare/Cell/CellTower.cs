using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Taxishare.Cell
{
    class CellTower
    {
        private int TowerId;
        private int LocationAreaCode;
        private int MobileCountryCode;
        private int MobileNetworkCode;

        public int getTowerId()
        {
            return TowerId;
        }
        public void setTowerId(int ti)
        {
            TowerId = ti;
        }

        public int getLocationAreaCode()
        {
            return LocationAreaCode;
        }
        public void setLocationAreaCode(int lcc)
        {
            LocationAreaCode = lcc;
        }

        public int getMobileCountryCode()
        {
            return MobileCountryCode;
        }
        public void setMobileCountryCode(int mcc)
        {
            MobileCountryCode = mcc;
        }

        public int getMobileNetworkCode()
        {
            return MobileNetworkCode;
        }
        public void setMobileNetworkCode(int mnc)
        {
            MobileNetworkCode = mnc;
        }
    }
}
