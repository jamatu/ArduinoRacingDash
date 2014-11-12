using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;

namespace iRacingSLI
{
    class brakeVibe
    {
        double old_ratio, ratio, rpm, speed, brake, thres, sens;
        int send;

        public brakeVibe()
        {
            
        }

        //Thanks to Stephane Turpin for locking algorithm
        public int getBrakeVibe(TelemetryInfo telem, int trkTol, int trkSens){
            speed = Convert.ToDouble(telem.Speed.Value);
            rpm = Convert.ToDouble(telem.RPM.Value);
            brake = Convert.ToDouble(telem.Brake.Value);

            //Calculates the ratio between the RPM and car speed
            //If RPM is minimum and the car is still moving -> we probably have locked tires
            if (speed > 1) {
                old_ratio = ratio;
                ratio = rpm / speed;
            } else {
                old_ratio = 0;
                ratio = 0;
            }
                
            //Check if we are pressing the breakes and the ratio is increasing (same RPM and less speed = higher ratio)
            send = 0;
            thres = (double)trkTol / 100D;
            sens = 0.010 - ((double)trkSens / 1000D);

            if (brake > thres) {
                if ((ratio > old_ratio) || (Math.Abs((ratio - old_ratio) / ratio) > sens)) {
                    send = 1;
                }
            }
            return send;
        }

    }
}
