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

        public void fetch(TelemetryInfo telem, iRacingSDK sdk, double fuelVal)
        {           
            Gear = telem.Gear.Value;
            Speed = telem.Speed.Value;
            RPM = Convert.ToInt16(telem.RPM.Value);
            Shift = telem.ShiftIndicatorPct.Value;
            Lap = telem.Lap.Value > 199 ? 199 : telem.Lap.Value;
            Engine = (byte)(Convert.ToString(telem.EngineWarnings.Value).Contains("PitSpeedLimiter") ? 1 : 0);
            DeltaNeg = 0;

            if (fuelVal != 0)
            {
                double tmp = Math.Round(telem.FuelLevel.Value/fuelVal, 2);
                if (tmp > 99.9)
                    Fuel = Convert.ToInt16(Math.Round(tmp));
                else if(tmp > 9.99){
                    Fuel = Convert.ToInt16(Math.Round(tmp * 10));
                    Engine |= 1 << 1;
                }
                else
                {
                    Fuel = Convert.ToInt16(Math.Round(tmp * 100));
                    Engine |= 2 << 1;
                }

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
            Delta = Delta > 9999 ? 9999 : Delta;
        }

        public byte[] compile(Boolean spdUnit, int intensity){

            int iSpeed = spdUnit ? Convert.ToInt16(Speed * 2.23693629) : Convert.ToInt16(Speed * (2.23693629 * 1.609344));
            int iShift = Convert.ToInt16(Math.Round((Shift * 100 * 16) / 100));

            serialdata[0] = 255;
            serialdata[1] = Convert.ToByte((DeltaNeg << 7) | (intensity << 4) | 0);
            serialdata[2] = Convert.ToByte(Gear + 1);
            serialdata[3] = Convert.ToByte((iSpeed >> 8) & 0x00FF);
            serialdata[4] = Convert.ToByte(iSpeed & 0x00FF);
            serialdata[5] = Convert.ToByte((RPM >> 8) & 0x00FF);
            serialdata[6] = Convert.ToByte(RPM & 0x00FF);
            serialdata[7] = Convert.ToByte((Fuel >> 8) & 0x00FF);
            serialdata[8] = Convert.ToByte(Fuel & 0x00FF);
            serialdata[9] = Convert.ToByte(iShift);
            serialdata[10] = Engine;
            serialdata[11] = Convert.ToByte(Lap);
            serialdata[12] = 0;
            serialdata[13] = Convert.ToByte((Delta >> 8) & 0x00FF);
            serialdata[14] = Convert.ToByte(Delta & 0x00FF);
            return serialdata;
        }

    }
}
