using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using iRSDKSharp;

namespace iRacingSLI
{
    class dataPacket
    {
        Action<String> console;
        byte[] serialdata = new byte[15];

        int Gear, RPM, Lap, Fuel, Delta, DeltaNeg;
        float Speed, Shift;
        byte Engine;


        public dataPacket(Action<String> callConsole)
        {
            console = callConsole;
        }

        public void fetch(TelemetryInfo telem, iRacingSDK sdk, Boolean fuelDisp)
        {
            Engine = 0;
            DeltaNeg = 0;
            
            Gear = telem.Gear.Value;
            Speed = telem.Speed.Value;
            RPM = Convert.ToInt16(telem.RPM.Value);
            Shift = telem.ShiftIndicatorPct.Value;
            Lap = telem.Lap.Value;
            if (Lap > 199)
                Lap = 199;
            if (Convert.ToString(telem.EngineWarnings.Value).Contains("PitSpeedLimiter"))
                Engine = 1;

            if (fuelDisp)
            {
                Fuel = 0;
            }
            else
            {
                Fuel = Convert.ToInt16(Math.Round(telem.FuelLevelPct.Value * 100));
                Engine |= 3 << 1;
            }

            Delta = (int)(Math.Round(Convert.ToSingle(sdk.GetData("LapDeltaToBestLap")) * 1000));
            if (Delta <= 0)
            { 
                DeltaNeg = 1;
                Delta = Delta * -1;
            }
            if (Delta > 9999)
                Delta = 9999;
        }

        public byte[] compile(Boolean spdUnit, int intensity){

            int iSpeed = 0;
            if (spdUnit)
                iSpeed = Convert.ToInt16(Speed * (2.23693629 * 1.609344));
            else
                iSpeed = Convert.ToInt16(Speed * 2.23693629);

            serialdata[0] = 255;
            serialdata[1] = Convert.ToByte((DeltaNeg << 7) | (intensity << 4) | 0);
            serialdata[2] = Convert.ToByte(Gear + 1);
            serialdata[3] = Convert.ToByte((iSpeed >> 8) & 0x00FF);
            serialdata[4] = Convert.ToByte(iSpeed & 0x00FF);
            serialdata[5] = Convert.ToByte((RPM >> 8) & 0x00FF);
            serialdata[6] = Convert.ToByte(RPM & 0x00FF);
            serialdata[7] = Convert.ToByte((Fuel >> 8) & 0x00FF);
            serialdata[8] = Convert.ToByte(Fuel & 0x00FF);
            serialdata[9] = Convert.ToByte(Math.Round((Shift * 100 * 16) / 100));
            serialdata[10] = Engine;
            serialdata[11] = Convert.ToByte(Lap);
            serialdata[12] = 0;
            serialdata[13] = Convert.ToByte((Delta >> 8) & 0x00FF);
            serialdata[14] = Convert.ToByte(Delta & 0x00FF);
            return serialdata;
        }

    }
}
