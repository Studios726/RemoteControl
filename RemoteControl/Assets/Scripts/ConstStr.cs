using UnityEngine;

public static class ConstStr
{
    #region res
    public const string CONNECTION_CONFIG_TABLE = "Assets/Res/Config/connection_config.asset";

    public const string CANVAS_PREFAB = "Assets/Res/UI/CanvasPrefab.prefab";
    public const string VIEW_PREFAB = "Assets/Res/UI/ViewPrefab.prefab";
    public const string EVENTSYSTEM_PREFAB = "Assets/Res/UI/EventSystemPrefab.prefab";
    public const string ACCOUNT_RECORD_PREFAB = "UI/AccountRecord";
    public const string OPERATION_RECORD_PREFAB = "Assets/Res/UI/UI/Settings/OperationRecord.prefab";
    public const string ACCOUNT_OPERATION_RECORD_PREFAB = "UI/AccountOperationRecord";
    public const string ACCOUNT_TAB_ON = "Image/AccountOn";
    public const string ACCOUNT_TAB_OFF = "Image/AccountOff";
    public const string MANAGER_TAB_ON = "Image/ManagerOn";
    public const string MANAGER_TAB_OFF = "Image/ManagerOff";
    public const string OPERATION_TAB_ON = "Assets/Res/UI/UI/Settings/OperationOn.png";
    public const string OPERATION_TAB_OFF = "Assets/Res/UI/UI/Settings/OperationOff.png";
    #endregion

    #region subject
    public const string LOADING_PROGRESS_SUBJECT = "LoadingProgress";
    public const string AREA_SELECT_SUBJECT = "AreaSelect";
    public const string ERROR_INFO_SUBJECT = "ErrorInfo";
    public const string TASK_PROGRESS_SUBJECT = "TaskProgress";
    public const string RUNTIME_DATA_SUBJECT = "RuntimeData";
    public const string COAL_HEAP_SCAN_SUBJECT = "CoalHeapScan";
    public const string LOGIN_SUBJECT = "Login";
    #endregion

    #region dataName

    #region table name
    public const string DATABASE_LOGIN_TABLE = "login";
    public const string DATABASE_ACCOUNTOPERATION_TABLE = "accountoperation";
    public const string DATABASE_VARIABLE_TABLE = "plcvariables";
    public const string DATABASE_HISTORY_TASK_MC = "history_task_mc";
    #endregion

    #region login
    public const string DATA_USERNAME = "Account";
    public const string DATA_PASSWORD = "Password";
    public const string DATA_IS_ADMIN = "IsAdmin";
    public const string DATA_INDEX = "Index";
    public const string DATA_DEPARTMENT = "Department";
    public const string DATA_JOB = "Job";
    public const string DATA_DEFAULT_PASSWORD = "DefaultPassword";
    public const string DATA_NAME = "UserName";
    #endregion
    #region history_task_mc
    public const string DATA_TASK_ID = "TaskID";
    public const string DATA_OPERATO_RSYSTEM = "OperatorSystem";
    public const string DATA_TASK_CREATE_TIME = "TaskCreateTime";
    public const string DATA_MACHINE = "Machine";
    public const string DATA_TASK_TYPE = "TaskType";
    public const string DATA_MATERIAL_RANGE_START = "MaterialRangeStart";
    public const string DATA_MATERIAL_RANGE_END = "MaterialRangeEnd";
    public const string DATA_SIDE_SELECTION = "SideSelection";
    public const string DATA_LEFT_RIGHT_RANGE_START = "LeftRightRangeStart";
    public const string DATA_LEFT_RIGHT_RANGE_END = "LeftRightRangeEnd";
    public const string DATA_STEP_LENGTH = "StepLength";
    public const string DATA_IS_TIMED = "IsTimed";
    public const string DATA_TIMEDAT = "TimedAt";
    public const string DATA_IS_QUANTIFIED = "IsQuantified";
    public const string DATA_QUANTITY = "Quantity";
    public const string DATA_OPERATOR = "Operator";
    public const string DATA_TASK_STATE = "TaskState";
    #endregion
    #region
    public const string DATA_ACCOUNT_OPERATION_INDEX = "Index";
    public const string DATA_ACCOUNT_OPERATION_TIME = "Time";
    public const string DATA_ACCOUNT_OPERATION_OPERATOR = "Operator";
    public const string DATA_ACCOUNT_OPERATION_DETAIL = "Detail";
    #endregion

    #region plc variables
    public const string DATA_TIME_STAMP = "TimeStamp";
    public const string DATA_S_NO = "sNO";
    public const string DATA_MATERIAL_STACKING_ROTARY_INVERTER_OPERATION_MONITORING = "MaterialStackingRotaryInverterOperationMonitoring";
    public const string DATA_MATERIAL_STACKING_ROTARY_MAIN_CIRCUIT_BREAKER = "MaterialStackingRotaryMainCircuitBreaker";
    public const string DATA_MATERIAL_STACKING_ROTARY_BRAKE_OPERATION_MONITORING = "MaterialStackingRotaryBrakeOperationMonitoring";
    public const string DATA_MATERIAL_STACKING_ROTARY_INVERTER_FAULT = "MaterialStackingRotaryInverterFault";
    public const string DATA_AMPLITUDE_INVERTER_OPERATION = "AmplitudeInverterOperation";
    public const string DATA_AMPLITUDE_PRIMARY_BRAKE_OPERATION = "AmplitudePrimaryBrakeOperation";
    public const string DATA_AMPLITUDE_BACKUP = "AmplitudeBackup";
    public const string DATA_AMPLITUDE_SECONDARY_BRAKE_OPERATION_MONITORING = "AmplitudeSecondaryBrakeOperationMonitoring";
    public const string DATA_AMPLITUDE_PRIMARY_BRAKE_OPERATION_MONITORING = "AmplitudePrimaryBrakeOperationMonitoring";
    public const string DATA_AMPLITUDE_MOTOR_OVERHEAT_PROTECTION = "AmplitudeMotorOverheatProtection";
    public const string DATA_AMPLITUDE_MOTOR_MAIN_CIRCUIT_BREAKER = "AmplitudeMotorMainCircuitBreaker";
    public const string DATA_MATERIAL_CONVEYOR_MOTOR_MAIN_CIRCUIT_BREAKER = "MaterialConveyorMotorMainCircuitBreaker";
    public const string DATA_MATERIAL_CONVEYOR_MOTOR_PROTECTION = "MaterialConveyorMotorProtection";
    public const string DATA_MATERIAL_CONVEYOR_MATERIAL_OPERATION_MONITORING = "MaterialConveyorMaterialOperationMonitoring";
    public const string DATA_MATERIAL_STACKING_ROTARY_BRAKE_OPERATION = "MaterialStackingRotaryBrakeOperation";
    public const string DATA_MATERIAL_STACKING_ROTARY_INVERTER_OPERATION = "MaterialStackingRotaryInverterOperation";
    public const string DATA_MATERIAL_STACKING_ROTARY_TURN_LEFT = "MaterialStackingRotaryTurnLeft";
    public const string DATA_MATERIAL_STACKING_ROTARY_TURN_RIGHT = "MaterialStackingRotaryTurnRight";
    public const string DATA_MATERIAL_STACKING_ROTARY_INVERTER_FAULT_RESET = "MaterialStackingRotaryInverterFaultReset";
    public const string DATA_MATERIAL_BELT_OPERATION = "MaterialBeltOperation";
    public const string DATA_REMOTE_CONTROL_POWER_DISCONNECT = "RemoteControlPowerDisconnect";
    public const string DATA_DUST_SUPPRESSION_STARTUP_SWITCH = "DustSuppressionStartupSwitch";
    public const string DATA_AMPLITUDE_LOWER = "AmplitudeLower";
    public const string DATA_LOW_VOLTAGE_CONTROL_POWER_CLOSURE = "LowVoltageControlPowerClosure";
    public const string DATA_LOW_VOLTAGE_POWER_CLOSURE = "LowVoltagePowerClosure";
    public const string DATA_ELECTRICAL_ROOM_EMERGENCY_STOP_BUTTON = "ElectricalRoomEmergencyStopButton";
    public const string DATA_EMERGENCY_STOP_RELAY = "EmergencyStopRelay";
    public const string DATA_LIGHTING_POWER_CLOSURE = "LightingPowerClosure";
    public const string DATA_SCRAPER_MOTOR_MAIN_CIRCUIT_BREAKER = "ScraperMotorMainCircuitBreaker";
    public const string DATA_DRIVER_CABIN_EMERGENCY_STOP_BUTTON = "DriverCabinEmergencyStopButton";
    public const string DATA_DRIVER_CABIN_SIGNAL_RESET_BUTTON = "DriverCabinSignalResetButton";
    public const string DATA_MATERIAL_CONTROL_AUTO_STACKING = "MaterialControlAutoStacking";
    public const string DATA_MATERIAL_CONTROL_MANUAL_FETCHING = "MaterialControlManualFetching";
    public const string DATA_MATERIAL_CONTROL_AUTO_FETCHING = "MaterialControlAutoFetching";
    public const string DATA_AUTO_STACKING_STARTUP_SWITCH = "AutoStackingStartupSwitch";
    public const string DATA_FAULT_ALARM = "FaultAlarm";
    public const string DATA_BYPASS_BUTTON = "BypassButton";
    public const string DATA_EQUIPMENT_FETCHING_OPERATION = "EquipmentFetchingOperation";
    public const string DATA_CIRCULAR_STACK_FAULT = "CircularStackFault";
    public const string DATA_SCRAPER_TENSION_LIMIT_SWITCH = "ScraperTensionLimitSwitch";
    public const string DATA_SCRAPER_LOOSE_LIMIT_SWITCH = "ScraperLooseLimitSwitch";
    public const string DATA_SCRAPER_REDUCER_OIL_PRESSURE_SWITCH_1 = "ScraperReducerOilPressureSwitch1";
    public const string DATA_SCRAPER_REDUCER_OIL_PRESSURE_SWITCH_2 = "ScraperReducerOilPressureSwitch2";
    public const string DATA_SCRAPER_FLUID_COUPLING_TEMPERATURE_SWITCH_2 = "ScraperFluidCouplingTemperatureSwitch2";
    public const string DATA_SCRAPER_MOTOR_2_OPERATION = "ScraperMotor2Operation";
    public const string DATA_REDUCER_LUBRICATION_OIL_PUMP_OPERATION = "ReducerLubricationOilPumpOperation";
    public const string DATA_MATERIAL_FETCHING_ROTARY_MAIN_CIRCUIT_BREAKER = "MaterialFetchingRotaryMainCircuitBreaker";
    public const string DATA_MATERIAL_FETCHING_ROTARY_MOTOR_CIRCUIT_BREAKER = "MaterialFetchingRotaryMotorCircuitBreaker";
    public const string DATA_MATERIAL_FETCHING_ROTARY_BRAKE_CIRCUIT_BREAKER = "MaterialFetchingRotaryBrakeCircuitBreaker";
    public const string DATA_MATERIAL_FETCHING_ROTARY_INVERTER_OPERATION_MONITORING = "MaterialFetchingRotaryInverterOperationMonitoring";
    public const string DATA_MATERIAL_FETCHING_ROTARY_BRAKE_OPERATION_MONITORING = "MaterialFetchingRotaryBrakeOperationMonitoring";
    public const string DATA_MATERIAL_FETCHING_ROTARY_INVERTER_FAULT = "MaterialFetchingRotaryInverterFault";
    public const string DATA_MATERIAL_FETCHING_ROTARY_INVERTER_OPERATION = "MaterialFetchingRotaryInverterOperation";
    public const string DATA_MATERIAL_FETCHING_ROTARY_BRAKE_OPERATION = "MaterialFetchingRotaryBrakeOperation";
    public const string DATA_MATERIAL_FETCHING_ROTARY_POSITION_CALIBRATION_LIMIT_SWITCH = "MaterialFetchingRotaryPositionCalibrationLimitSwitch";
    public const string DATA_AMPLITUDE_BRAKE_RESISTOR_OVERHEAT_SWITCH = "AmplitudeBrakeResistorOverheatSwitch";
    public const string DATA_AMPLITUDE_WEIGHT_LIMITER_OVERLOAD_ALARM_1 = "AmplitudeWeightLimiterOverloadAlarm1";
    public const string DATA_MATERIAL_FETCHING_ROTARY_TURN_LEFT = "MaterialFetchingRotaryTurnLeft";
    public const string DATA_MATERIAL_FETCHING_ROTARY_TURN_RIGHT = "MaterialFetchingRotaryTurnRight";
    public const string DATA_MATERIAL_FETCHING_ROTARY_INVERTER_FAULT_RESET = "MaterialFetchingRotaryInverterFaultReset";
    public const string DATA_AMPLITUDE_AIR_COOLED_MOTOR_OPERATION_MONITORING = "AmplitudeAirCooledMotorOperationMonitoring";
    public const string DATA_AMPLITUDE_INVERTER_FAULT = "AmplitudeInverterFault";
    public const string DATA_AMPLITUDE_BRAKE_OPERATION_SIGNAL = "AmplitudeBrakeOperationSignal";
    public const string DATA_AMPLITUDE_UP_LIMIT_SWITCH = "AmplitudeUpLimitSwitch";
    public const string DATA_AMPLITUDE_UP_EXTREME_LIMIT = "AmplitudeUpExtremeLimit";
    public const string DATA_AMPLITUDE_LOWER_LIMIT_SWITCH = "AmplitudeLowerLimitSwitch";
    public const string DATA_MANUAL_CONTROL = "ManualControl";
    public const string DATA_REMOTE_POWER_CLOSURE = "RemotePowerClosure";
    public const string DATA_REMOTE_POWER_DISCONNECT = "RemotePowerDisconnect";
    public const string DATA_REMOTE_LIGHTING_POWER_CLOSURE = "RemoteLightingPowerClosure";
    public const string DATA_AMPLITUDE_INVERTER_OPERATION_MONITORING = "AmplitudeInverterOperationMonitoring";
    public const string DATA_AMPLITUDE_SECONDARY_BRAKE_OPERATION = "AmplitudeSecondaryBrakeOperation";
    public const string DATA_AMPLITUDE_INVERTER_FAULT_RESET = "AmplitudeInverterFaultReset";
    public const string DATA_AMPLITUDE_AIR_COOLED_MOTOR_OPERATION = "AmplitudeAirCooledMotorOperation";
    public const string DATA_DB_23_W_0 = "DB23_W0";
    public const string DATA_DB_26_W_0 = "DB26_W0";
    public const string DATA_DB_27_W_0 = "DB27_W0";
    public const string DATA_DB_25_W_0 = "DB25_W0";
    public const string DATA_DB_24_W_0 = "DB24_W0";
    public const string DATA_DB_22_W_0 = "DB22_W0";
    public const string DATA_DB_21_W_0 = "DB21_W0";
    public const string DATA_HMI_A_W_0 = "HMI_a_W0";
    public const string DATA_ELECTRICAL_ROOM_FIRE_ALARM_SIGNAL = "ElectricalRoomFireAlarmSignal";
    public const string DATA_ELECTRICAL_ROOM_SIGNAL_RESET_BUTTON = "ElectricalRoomSignalResetButton";
    public const string DATA_CONTROL_MODE_LOCAL_CONTROL = "ControlModeLocalControl";
    public const string DATA_CONTROL_MODE_REMOTE_CONTROL = "ControlModeRemoteControl";
    public const string DATA_MATERIAL_CONTROL_MANUAL_STACKING = "MaterialControlManualStacking";
    public const string DATA_MATERIAL_FETCHING_ROTARY_TURN_LEFT_SWITCH = "MaterialFetchingRotaryTurnLeftSwitch";
    public const string DATA_MATERIAL_FETCHING_ROTARY_TURN_RIGHT_SWITCH = "MaterialFetchingRotaryTurnRightSwitch";
    public const string DATA_INTERLOCK_SWITCH_WITH_SYSTEM = "InterlockSwitchWithSystem";
    public const string DATA_STARTUP_ALARM_SWITCH = "StartupAlarmSwitch";
    public const string DATA_MATERIAL_BELT_STARTUP_RUN_SWITCH = "MaterialBeltStartupRunSwitch";
    public const string DATA_MATERIAL_FETCHING_ROTARY_SPEED_SELECTION_SWITCH = "MaterialFetchingRotarySpeedSelectionSwitch";
    public const string DATA_AMPLITUDE_UP_SWITCH = "AmplitudeUpSwitch";
    public const string DATA_MATERIAL_STACKING_ROTARY_TURN_LEFT_SWITCH = "MaterialStackingRotaryTurnLeftSwitch";
    public const string DATA_MATERIAL_STACKING_ROTARY_SPEED_SELECTION_SWITCH = "MaterialStackingRotarySpeedSelectionSwitch";
    public const string DATA_AMPLITUDE_UP = "AmplitudeUp";
    public const string DATA_AMPLITUDE_LOWER_SWITCH = "AmplitudeLowerSwitch";
    public const string DATA_AMPLITUDE_SPEED_SELECTION_SWITCH = "AmplitudeSpeedSelectionSwitch";
    public const string DATA_ELECTRICAL_ROOM_P_L_C_MODULE_POWER = "ElectricalRoomPLCModulePower";
    public const string DATA_DRIVER_CABIN_FIRE_ALARM_SIGNAL = "DriverCabinFireAlarmSignal";
    public const string DATA_AUTO_FETCHING_STARTUP_SWITCH = "AutoFetchingStartupSwitch";
    public const string DATA_AMPLITUDE_LOWER_EXTREME = "AmplitudeLowerExtreme";
    public const string DATA_MATERIAL_MECHANISM_AUDIO_VISUAL_ALARM = "MaterialMechanismAudioVisualAlarm";
    public const string DATA_EQUIPMENT_STACKING_OPERATION = "EquipmentStackingOperation";
    public const string DATA_SCRAPER_MOTOR_1_OPERATION = "ScraperMotor1Operation";
    public const string DATA_REMOTE_EMERGENCY_STOP_CONTROL = "RemoteEmergencyStopControl";
    public const string DATA_AUTO_CONTROL = "AutoControl";
    public const string DATA_EQUIPMENT_FAULT = "EquipmentFault";
    public const string DATA_M_SCRAPER_MOTOR_1_CURRENT = "M_ScraperMotor1Current";
    public const string DATA_M_SCRAPER_MOTOR_2_CURRENT = "M_ScraperMotor2Current";
    public const string DATA_M_MATERIAL_FETCHING_AMPLITUDE_MOTOR_CURRENT = "M_MaterialFetchingAmplitudeMotorCurrent";
    public const string DATA_M_MATERIAL_FETCHING_ROTARY_MOTOR_CURRENT = "M_MaterialFetchingRotaryMotorCurrent";
    public const string DATA_M_MATERIAL_STACKING_BELT_MOTOR_CURRENT = "M_MaterialStackingBeltMotorCurrent";
    public const string DATA_M_MATERIAL_STACKING_ROTARY_MOTOR_CURRENT = "M_MaterialStackingRotaryMotorCurrent";
    public const string DATA_MATERIAL_FETCHING_ROTARY_ANGLE = "MaterialFetchingRotaryAngle";
    public const string DATA_MATERIAL_FETCHING_PITCH_ANGLE = "MaterialFetchingPitchAngle";
    public const string DATA_MATERIAL_STACKING_ROTARY_ANGLE = "MaterialStackingRotaryAngle";
    public const string DATA_AUTO_STACKING_BELT_OPERATION = "AutoStackingBeltOperation";
    public const string DATA_AUTO_STACKING_FORCE_STOP = "AutoStackingForceStop";
    public const string DATA_AUTO_STACKING_TOUCH_SCREEN_STARTUP = "AutoStackingTouchScreenStartup";
    public const string DATA_AUTO_STACKING_TOUCH_SCREEN_STOP = "AutoStackingTouchScreenStop";
    public const string DATA_AUTO_STACKING_TOUCH_SCREEN_TURN_LEFT_SETTING = "AutoStackingTouchScreenTurnLeftSetting";
    public const string DATA_AUTO_STACKING_TOUCH_SCREEN_TURN_RIGHT_SETTING = "AutoStackingTouchScreenTurnRightSetting";
    public const string DATA_AUTO_STACKING_LEFT_TURN_SIGNAL = "AutoStackingLeftTurnSignal";
    public const string DATA_AUTO_STACKING_RIGHT_TURN_SIGNAL = "AutoStackingRightTurnSignal";
    public const string DATA_AUTO_STACKING_LEFT_TURN = "AutoStackingLeftTurn";
    public const string DATA_AUTO_STACKING_RIGHT_TURN = "AutoStackingRightTurn";
    public const string DATA_M_AUTO_STACKING_RANGE_SETTING = "M_AutoStackingRangeSetting";
    public const string DATA_MANUAL_INTERVENTION = "ManualIntervention";
    public const string DATA_INTERVENTION_STOP = "InterventionStop";
    public const string DATA_AUTO_STACKING_INTERVENTION = "AutoStackingIntervention";
    public const string DATA_AUTO_FETCHING_INTERVENTION = "AutoFetchingIntervention";
    public const string DATA_AUTO_EDGE_FETCHING = "AutoEdgeFetching";
    public const string DATA_AUTO_FETCHING_TOUCH_SCREEN_STARTUP = "AutoFetchingTouchScreenStartup";
    public const string DATA_AUTO_FETCHING_TOUCH_SCREEN_STOP = "AutoFetchingTouchScreenStop";
    public const string DATA_TOUCH_SCREEN_EDGE_FETCHING = "TouchScreenEdgeFetching";
    public const string DATA_TOUCH_SCREEN_MIDDLE_FETCHING = "TouchScreenMiddleFetching";
    public const string DATA_AUTO_FETCHING_TOUCH_SCREEN_TURN_LEFT_SETTING = "AutoFetchingTouchScreenTurnLeftSetting";
    public const string DATA_AUTO_FETCHING_TOUCH_SCREEN_TURN_RIGHT_SETTING = "AutoFetchingTouchScreenTurnRightSetting";
    public const string DATA_AUTO_FETCHING_LEFT_TURN_SIGNAL = "AutoFetchingLeftTurnSignal";
    public const string DATA_AUTO_FETCHING_RIGHT_TURN_SIGNAL = "AutoFetchingRightTurnSignal";
    public const string DATA_EDGE_FETCHING_LEFT_TURN = "EdgeFetchingLeftTurn";
    public const string DATA_EDGE_FETCHING_RIGHT_TURN = "EdgeFetchingRightTurn";
    public const string DATA_AUTO_MIDDLE_FETCHING = "AutoMiddleFetching";
    public const string DATA_MIDDLE_FETCHING_LEFT_TURN = "MiddleFetchingLeftTurn";
    public const string DATA_MIDDLE_FETCHING_RIGHT_TURN = "MiddleFetchingRightTurn";
    public const string DATA_AUTO_FETCHING_SCRAPER_OPERATION = "AutoFetchingScraperOperation";
    public const string DATA_AUTO_FETCHING_ROTARY_IN_PLACE = "AutoFetchingRotaryInPlace";
    public const string DATA_AUTO_FETCHING_SCRAPER_DESCEND = "AutoFetchingScraperDescend";
    public const string DATA_M_AUTO_EDGE_FETCHING_ANGLE_SETTING = "M_AutoEdgeFetchingAngleSetting";
    public const string DATA_M_AUTO_MIDDLE_FETCHING_ANGLE_SETTING = "M_AutoMiddleFetchingAngleSetting";
    public const string DATA_SCRAPER_LUBRICATION_BUTTON = "ScraperLubricationButton";
    public const string DATA_SCRAPER_FRAME_LUBRICATION_BUTTON = "ScraperFrameLubricationButton";
    public const string DATA_MATERIAL_STACKING_UPPER_LUBRICATION_BUTTON = "MaterialStackingUpperLubricationButton";
    public const string DATA_MATERIAL_STACKING_MIDDLE_LUBRICATION_BUTTON = "MaterialStackingMiddleLubricationButton";
    public const string DATA_MATERIAL_FETCHING_WIRE_ROPE_LUBRICATION_BUTTON = "MaterialFetchingWireRopeLubricationButton";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LUBRICATION_BUTTON = "MaterialFetchingRotaryLubricationButton";
    public const string DATA_AMPLITUDE_BRAKE_OPENING_LIMIT = "AmplitudeBrakeOpeningLimit";
    public const string DATA_SCRAPER_HYDRAULIC_TENSIONER_FAULT = "ScraperHydraulicTensionerFault";
    public const string DATA_MATERIAL_STACKING_BELT_BRAKE_OPERATION_MONITORING = "MaterialStackingBeltBrakeOperationMonitoring";
    public const string DATA_MATERIAL_FETCHING_BEARING_LUBRICATION_BUTTON = "MaterialFetchingBearingLubricationButton";
    public const string DATA_MATERIAL_FETCHING_ROTARY_BACKUP = "MaterialFetchingRotaryBackup";
    public const string DATA_MATERIAL_STACKING_ROTARY_BACKUP = "MaterialStackingRotaryBackup";
    public const string DATA_SCRAPER_TEMPERATURE_SWITCH_1 = "ScraperTemperatureSwitch1";
    public const string DATA_TOUCH_SCREEN_TENSION_MANUAL_STARTUP_BUTTON = "TouchScreenTensionManualStartupButton";
    public const string DATA_TOUCH_SCREEN_TENSION_MANUAL_STOP_BUTTON = "TouchScreenTensionManualStopButton";
    public const string DATA_REMOTE_CONTROL_POWER_CLOSURE = "RemoteControlPowerClosure";
    public const string DATA_SCRAPER_MOTOR_OVERHEAT_PROTECTION = "ScraperMotorOverheatProtection";
    public const string DATA_SCRAPER_MOTOR_1_OPERATION_MONITORING = "ScraperMotor1OperationMonitoring";
    public const string DATA_SCRAPER_MOTOR_2_OPERATION_MONITORING = "ScraperMotor2OperationMonitoring";
    public const string DATA_SCRAPER_REDUCER_LUBRICATION_OIL_PUMP_MONITORING = "ScraperReducerLubricationOilPumpMonitoring";
    public const string DATA_MATERIAL_FETCHING_COAL_BUCKET_BLOCKAGE_SWITCH = "MaterialFetchingCoalBucketBlockageSwitch";
    public const string DATA_MATERIAL_FETCHING_ROTARY_BRAKE_RESISTOR_TEMPERATURE_ALARM = "MaterialFetchingRotaryBrakeResistorTemperatureAlarm";
    public const string DATA_AMPLITUDE_WEIGHT_LIMITER_OVERLOAD_ALARM_2 = "AmplitudeWeightLimiterOverloadAlarm2";
    public const string DATA_MATERIAL_STACKING_ROTARY_TURN_RIGHT_SWITCH = "MaterialStackingRotaryTurnRightSwitch";
    public const string DATA_MATERIAL_STACKING_BELT_PRIMARY_DEVIATION = "MaterialStackingBeltPrimaryDeviation";
    public const string DATA_MATERIAL_STACKING_BELT_SECONDARY_DEVIATION = "MaterialStackingBeltSecondaryDeviation";
    public const string DATA_MATERIAL_STACKING_BELT_ROPE_PROTECTION = "MaterialStackingBeltRopeProtection";
    public const string DATA_MATERIAL_STACKING_BELT_SPEED_DETECTION = "MaterialStackingBeltSpeedDetection";
    public const string DATA_MATERIAL_STACKING_BELT_MATERIAL_FLOW_DETECTION = "MaterialStackingBeltMaterialFlowDetection";
    public const string DATA_MATERIAL_STACKING_BELT_LONGITUDINAL_TEAR_DETECTION = "MaterialStackingBeltLongitudinalTearDetection";
    public const string DATA_MATERIAL_STACKING_ARM_LEFT_COLLISION_SWITCH = "MaterialStackingArmLeftCollisionSwitch";
    public const string DATA_MATERIAL_STACKING_ARM_RIGHT_COLLISION_SWITCH = "MaterialStackingArmRightCollisionSwitch";
    public const string DATA_MATERIAL_STACKING_COAL_BUCKET_BLOCKAGE_SWITCH = "MaterialStackingCoalBucketBlockageSwitch";
    public const string DATA_SYSTEM_ALLOW_STACKING_COMMAND = "SystemAllowStackingCommand";
    public const string DATA_SYSTEM_ALLOW_FETCHING_COMMAND = "SystemAllowFetchingCommand";
    public const string DATA_SYSTEM_EMERGENCY_STOP_COMMAND = "SystemEmergencyStopCommand";
    public const string DATA_WATER_PUMP_MOTOR_OPERATION = "WaterPumpMotorOperation";
    public const string DATA_MATERIAL_STACKING_WATER_SPRAY_SOLENOID_VALVE_OPERATION = "MaterialStackingWaterSpraySolenoidValveOperation";
    public const string DATA_MATERIAL_STACKING_BELT_BRAKE_OPERATION = "MaterialStackingBeltBrakeOperation";
    public const string DATA_DB_32_W_0 = "DB32_W0";
    public const string DATA_UNATTENDED_EMERGENCY_STOP = "UnattendedEmergencyStop";
    public const string DATA_MATERIAL_FETCHING_BEARING_LUBRICATION_STOP_BUTTON = "MaterialFetchingBearingLubricationStopButton";
    public const string DATA_SCRAPER_FRAME_LUBRICATION_STOP_BUTTON = "ScraperFrameLubricationStopButton";
    public const string DATA_WATER_PUMP_MOTOR_MAIN_CIRCUIT_BREAKER = "WaterPumpMotorMainCircuitBreaker";
    public const string DATA_WATER_PUMP_MOTOR_OVERLOAD = "WaterPumpMotorOverload";
    public const string DATA_WATER_PUMP_MOTOR_CONTACTOR = "WaterPumpMotorContactor";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LEFT_TURN_LIMIT = "MaterialFetchingRotaryLeftTurnLimit";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LEFT_TURN_EXTREME_LIMIT = "MaterialFetchingRotaryLeftTurnExtremeLimit";
    public const string DATA_MATERIAL_FETCHING_ROTARY_RIGHT_TURN_LIMIT = "MaterialFetchingRotaryRightTurnLimit";
    public const string DATA_MATERIAL_FETCHING_ROTARY_RIGHT_TURN_EXTREME_LIMIT = "MaterialFetchingRotaryRightTurnExtremeLimit";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LEFT_COLLISION_SWITCH_1 = "MaterialFetchingRotaryLeftCollisionSwitch1";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LEFT_COLLISION_SWITCH_2 = "MaterialFetchingRotaryLeftCollisionSwitch2";
    public const string DATA_MATERIAL_FETCHING_ROTARY_RIGHT_COLLISION_SWITCH_1 = "MaterialFetchingRotaryRightCollisionSwitch1";
    public const string DATA_MATERIAL_FETCHING_ROTARY_RIGHT_COLLISION_SWITCH_2 = "MaterialFetchingRotaryRightCollisionSwitch2";
    public const string DATA_MATERIAL_STACKING_MIDDLE_LUBRICATION_OPERATION = "MaterialStackingMiddleLubricationOperation";
    public const string DATA_MATERIAL_STACKING_UPPER_LUBRICATION_OPERATION = "MaterialStackingUpperLubricationOperation";
    public const string DATA_MATERIAL_FETCHING_ROTARY_LUBRICATION_OPERATION = "MaterialFetchingRotaryLubricationOperation";
    public const string DATA_SCRAPER_LUBRICATION_OPERATION = "ScraperLubricationOperation";
    public const string DATA_MATERIAL_ROTATION_LUBRICATION_OPERATION = "MaterialRotationLubricationOperation";
    public const string DATA_SCRAPER_FRAME_LUBRICATION_OPERATION = "ScraperFrameLubricationOperation";
    public const string DATA_MATERIAL_BEARING_LUBRICATION_OPERATION = "MaterialBearingLubricationOperation";
    public const string DATA_FLUID_COUPLING_TEMPERATURE_SWITCH = "FluidCouplingTemperatureSwitch";
    public const string DATA_MATERIAL_PILE_MIDDLE_LUBRICATION_STOP_BUTTON = "MaterialPileMiddleLubricationStopButton";
    public const string DATA_MATERIAL_PILE_UPPER_LUBRICATION_STOP_BUTTON = "MaterialPileUpperLubricationStopButton";
    public const string DATA_MATERIAL_WIRE_ROPE_LUBRICATION_STOP_BUTTON = "MaterialWireRopeLubricationStopButton";
    public const string DATA_MATERIAL_ROTATION_LUBRICATION_STOP_BUTTON = "MaterialRotationLubricationStopButton";
    public const string DATA_SCRAPER_LUBRICATION_STOP_BUTTON = "ScraperLubricationStopButton";
    public const string DATA_STACKING_ROTATION_LEFT_ANTI_COLLISION_LIMIT = "StackingRotationLeftAntiCollisionLimit";
    public const string DATA_STACKING_ROTATION_RIGHT_ANTI_COLLISION_LIMIT = "StackingRotationRightAntiCollisionLimit";
    public const string DATA_SCRAPER_START_SWITCH = "ScraperStartSwitch";
    #endregion

    #endregion

    #region SystemCode
    public const string CONTROL_SYSTEM = "MC";
    public const string SCAN_SYSTEM = "SCAN";
    #endregion

    #region

    public const string PROJECT_NAME = "华润沈阳电厂斗轮机无人值守系统";


    #endregion
}

public enum Machine
{
    [Header("斗轮取料机")]
    BucketWheel,
    [Header("斗轮堆取料机")]
    BucketWheelStackerReclaimer
}