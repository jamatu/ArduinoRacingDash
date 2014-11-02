 
// This is the output code from your template
// you only get syntax-highlighting here - not intellisense

public class X
{
	

	/// <summary>
	/// Seconds since session start. Unit: s
	/// </summary>
	public TelemetryValue< double > SessionTime { get { return new TelemetryValue< double >(sdk, "SessionTime"); } }


	/// <summary>
	/// Session number. 
	/// </summary>
	public TelemetryValue< int > SessionNum { get { return new TelemetryValue< int >(sdk, "SessionNum"); } }


	/// <summary>
	/// Session state. Unit: irsdk_SessionState
	/// </summary>
	public TelemetryValue< int > SessionState { get { return new TelemetryValue< int >(sdk, "SessionState"); } }


	/// <summary>
	/// Session ID. 
	/// </summary>
	public TelemetryValue< int > SessionUniqueID { get { return new TelemetryValue< int >(sdk, "SessionUniqueID"); } }


	/// <summary>
	/// Session flags. Unit: irsdk_Flags
	/// </summary>
	public TelemetryValue< int > SessionFlags { get { return new TelemetryValue< int >(sdk, "SessionFlags"); } }


	/// <summary>
	/// Driver activated flag. 
	/// </summary>
	public TelemetryValue< bool > DriverMarker { get { return new TelemetryValue< bool >(sdk, "DriverMarker"); } }


	/// <summary>
	/// 0=replay not playing  1=replay playing. 
	/// </summary>
	public TelemetryValue< bool > IsReplayPlaying { get { return new TelemetryValue< bool >(sdk, "IsReplayPlaying"); } }


	/// <summary>
	/// Integer replay frame number (60 per second). 
	/// </summary>
	public TelemetryValue< int > ReplayFrameNum { get { return new TelemetryValue< int >(sdk, "ReplayFrameNum"); } }


	/// <summary>
	/// Lap count by car index. 
	/// </summary>
	public TelemetryValue< int[] > CarIdxLap { get { return new TelemetryValue< int[] >(sdk, "CarIdxLap"); } }


	/// <summary>
	/// Percentage distance around lap by car index. Unit: %
	/// </summary>
	public TelemetryValue< float[] > CarIdxLapDistPct { get { return new TelemetryValue< float[] >(sdk, "CarIdxLapDistPct"); } }


	/// <summary>
	/// Track surface type by car index. Unit: irsdk_TrkLoc
	/// </summary>
	public TelemetryValue< int[] > CarIdxTrackSurface { get { return new TelemetryValue< int[] >(sdk, "CarIdxTrackSurface"); } }


	/// <summary>
	/// Steering wheel angle by car index. Unit: rad
	/// </summary>
	public TelemetryValue< float[] > CarIdxSteer { get { return new TelemetryValue< float[] >(sdk, "CarIdxSteer"); } }


	/// <summary>
	/// Engine rpm by car index. Unit: revs/min
	/// </summary>
	public TelemetryValue< float[] > CarIdxRPM { get { return new TelemetryValue< float[] >(sdk, "CarIdxRPM"); } }


	/// <summary>
	/// -1=reverse  0=neutral  1..n=current gear by car index. 
	/// </summary>
	public TelemetryValue< int[] > CarIdxGear { get { return new TelemetryValue< int[] >(sdk, "CarIdxGear"); } }


	/// <summary>
	/// Steering wheel angle. Unit: rad
	/// </summary>
	public TelemetryValue< float > SteeringWheelAngle { get { return new TelemetryValue< float >(sdk, "SteeringWheelAngle"); } }


	/// <summary>
	/// 0=off throttle to 1=full throttle. Unit: %
	/// </summary>
	public TelemetryValue< float > Throttle { get { return new TelemetryValue< float >(sdk, "Throttle"); } }


	/// <summary>
	/// 0=brake released to 1=max pedal force. Unit: %
	/// </summary>
	public TelemetryValue< float > Brake { get { return new TelemetryValue< float >(sdk, "Brake"); } }


	/// <summary>
	/// 0=disengaged to 1=fully engaged. Unit: %
	/// </summary>
	public TelemetryValue< float > Clutch { get { return new TelemetryValue< float >(sdk, "Clutch"); } }


	/// <summary>
	/// -1=reverse  0=neutral  1..n=current gear. 
	/// </summary>
	public TelemetryValue< int > Gear { get { return new TelemetryValue< int >(sdk, "Gear"); } }


	/// <summary>
	/// Engine rpm. Unit: revs/min
	/// </summary>
	public TelemetryValue< float > RPM { get { return new TelemetryValue< float >(sdk, "RPM"); } }


	/// <summary>
	/// Lap count. 
	/// </summary>
	public TelemetryValue< int > Lap { get { return new TelemetryValue< int >(sdk, "Lap"); } }


	/// <summary>
	/// Meters traveled from S/F this lap. Unit: m
	/// </summary>
	public TelemetryValue< float > LapDist { get { return new TelemetryValue< float >(sdk, "LapDist"); } }


	/// <summary>
	/// Percentage distance around lap. Unit: %
	/// </summary>
	public TelemetryValue< float > LapDistPct { get { return new TelemetryValue< float >(sdk, "LapDistPct"); } }


	/// <summary>
	/// Laps completed in race. 
	/// </summary>
	public TelemetryValue< int > RaceLaps { get { return new TelemetryValue< int >(sdk, "RaceLaps"); } }


	/// <summary>
	/// Longitudinal acceleration (including gravity). Unit: m/s^2
	/// </summary>
	public TelemetryValue< float > LongAccel { get { return new TelemetryValue< float >(sdk, "LongAccel"); } }


	/// <summary>
	/// Lateral acceleration (including gravity). Unit: m/s^2
	/// </summary>
	public TelemetryValue< float > LatAccel { get { return new TelemetryValue< float >(sdk, "LatAccel"); } }


	/// <summary>
	/// Vertical acceleration (including gravity). Unit: m/s^2
	/// </summary>
	public TelemetryValue< float > VertAccel { get { return new TelemetryValue< float >(sdk, "VertAccel"); } }


	/// <summary>
	/// Roll rate. Unit: rad/s
	/// </summary>
	public TelemetryValue< float > RollRate { get { return new TelemetryValue< float >(sdk, "RollRate"); } }


	/// <summary>
	/// Pitch rate. Unit: rad/s
	/// </summary>
	public TelemetryValue< float > PitchRate { get { return new TelemetryValue< float >(sdk, "PitchRate"); } }


	/// <summary>
	/// Yaw rate. Unit: rad/s
	/// </summary>
	public TelemetryValue< float > YawRate { get { return new TelemetryValue< float >(sdk, "YawRate"); } }


	/// <summary>
	/// GPS vehicle speed. Unit: m/s
	/// </summary>
	public TelemetryValue< float > Speed { get { return new TelemetryValue< float >(sdk, "Speed"); } }


	/// <summary>
	/// X velocity. Unit: m/s
	/// </summary>
	public TelemetryValue< float > VelocityX { get { return new TelemetryValue< float >(sdk, "VelocityX"); } }


	/// <summary>
	/// Y velocity. Unit: m/s
	/// </summary>
	public TelemetryValue< float > VelocityY { get { return new TelemetryValue< float >(sdk, "VelocityY"); } }


	/// <summary>
	/// Z velocity. Unit: m/s
	/// </summary>
	public TelemetryValue< float > VelocityZ { get { return new TelemetryValue< float >(sdk, "VelocityZ"); } }


	/// <summary>
	/// Yaw orientation. Unit: rad
	/// </summary>
	public TelemetryValue< float > Yaw { get { return new TelemetryValue< float >(sdk, "Yaw"); } }


	/// <summary>
	/// Pitch orientation. Unit: rad
	/// </summary>
	public TelemetryValue< float > Pitch { get { return new TelemetryValue< float >(sdk, "Pitch"); } }


	/// <summary>
	/// Roll orientation. Unit: rad
	/// </summary>
	public TelemetryValue< float > Roll { get { return new TelemetryValue< float >(sdk, "Roll"); } }


	/// <summary>
	/// Active camera's focus car index. 
	/// </summary>
	public TelemetryValue< int > CamCarIdx { get { return new TelemetryValue< int >(sdk, "CamCarIdx"); } }


	/// <summary>
	/// Active camera number. 
	/// </summary>
	public TelemetryValue< int > CamCameraNumber { get { return new TelemetryValue< int >(sdk, "CamCameraNumber"); } }


	/// <summary>
	/// Active camera group number. 
	/// </summary>
	public TelemetryValue< int > CamGroupNumber { get { return new TelemetryValue< int >(sdk, "CamGroupNumber"); } }


	/// <summary>
	/// State of camera system. Unit: irsdk_CameraState
	/// </summary>
	public TelemetryValue< int > CamCameraState { get { return new TelemetryValue< int >(sdk, "CamCameraState"); } }


	/// <summary>
	/// 1=Car on track physics running. 
	/// </summary>
	public TelemetryValue< bool > IsOnTrack { get { return new TelemetryValue< bool >(sdk, "IsOnTrack"); } }


	/// <summary>
	/// 1=Car in garage physics running. 
	/// </summary>
	public TelemetryValue< bool > IsInGarage { get { return new TelemetryValue< bool >(sdk, "IsInGarage"); } }


	/// <summary>
	/// Output torque on steering shaft. Unit: N*m
	/// </summary>
	public TelemetryValue< float > SteeringWheelTorque { get { return new TelemetryValue< float >(sdk, "SteeringWheelTorque"); } }


	/// <summary>
	/// Force feedback % max torque on steering shaft. Unit: %
	/// </summary>
	public TelemetryValue< float > SteeringWheelPctTorque { get { return new TelemetryValue< float >(sdk, "SteeringWheelPctTorque"); } }


	/// <summary>
	/// Percent of shift indicator to light up. Unit: %
	/// </summary>
	public TelemetryValue< float > ShiftIndicatorPct { get { return new TelemetryValue< float >(sdk, "ShiftIndicatorPct"); } }


	/// <summary>
	/// Bitfield for warning lights. Unit: irsdk_EngineWarnings
	/// </summary>
	public TelemetryValue< int > EngineWarnings { get { return new TelemetryValue< int >(sdk, "EngineWarnings"); } }


	/// <summary>
	/// Liters of fuel remaining. Unit: l
	/// </summary>
	public TelemetryValue< float > FuelLevel { get { return new TelemetryValue< float >(sdk, "FuelLevel"); } }


	/// <summary>
	/// Percent fuel remaining. Unit: %
	/// </summary>
	public TelemetryValue< float > FuelLevelPct { get { return new TelemetryValue< float >(sdk, "FuelLevelPct"); } }


	/// <summary>
	/// Replay playback speed. 
	/// </summary>
	public TelemetryValue< int > ReplayPlaySpeed { get { return new TelemetryValue< int >(sdk, "ReplayPlaySpeed"); } }


	/// <summary>
	/// 0=not slow motion  1=replay is in slow motion. 
	/// </summary>
	public TelemetryValue< bool > ReplayPlaySlowMotion { get { return new TelemetryValue< bool >(sdk, "ReplayPlaySlowMotion"); } }


	/// <summary>
	/// Seconds since replay session start. Unit: s
	/// </summary>
	public TelemetryValue< double > ReplaySessionTime { get { return new TelemetryValue< double >(sdk, "ReplaySessionTime"); } }


	/// <summary>
	/// Replay session number. 
	/// </summary>
	public TelemetryValue< int > ReplaySessionNum { get { return new TelemetryValue< int >(sdk, "ReplaySessionNum"); } }


	/// <summary>
	/// Engine coolant temp. Unit: C
	/// </summary>
	public TelemetryValue< float > WaterTemp { get { return new TelemetryValue< float >(sdk, "WaterTemp"); } }


	/// <summary>
	/// Engine coolant level. Unit: l
	/// </summary>
	public TelemetryValue< float > WaterLevel { get { return new TelemetryValue< float >(sdk, "WaterLevel"); } }


	/// <summary>
	/// Engine fuel pressure. Unit: bar
	/// </summary>
	public TelemetryValue< float > FuelPress { get { return new TelemetryValue< float >(sdk, "FuelPress"); } }


	/// <summary>
	/// Engine oil temperature. Unit: C
	/// </summary>
	public TelemetryValue< float > OilTemp { get { return new TelemetryValue< float >(sdk, "OilTemp"); } }


	/// <summary>
	/// Engine oil pressure. Unit: bar
	/// </summary>
	public TelemetryValue< float > OilPress { get { return new TelemetryValue< float >(sdk, "OilPress"); } }


	/// <summary>
	/// Engine oil level. Unit: l
	/// </summary>
	public TelemetryValue< float > OilLevel { get { return new TelemetryValue< float >(sdk, "OilLevel"); } }


	/// <summary>
	/// Engine voltage. Unit: V
	/// </summary>
	public TelemetryValue< float > Voltage { get { return new TelemetryValue< float >(sdk, "Voltage"); } }

	
}

 
