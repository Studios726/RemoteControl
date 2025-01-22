using UnityEngine;
using XCharts.Runtime;

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
    public const string DATABASE_LOGIN_TABLE = "sys_user";
    public const string DATABASE_ACCOUNTOPERATION_TABLE = "accountoperation";
    public const string DATABASE_VARIABLE_TABLE = "plcvariables";
    public const string DATABASE_HISTORY_TASK_MC = "history_task_mc";
    public const string DATABASE_HISTORY_CARTELECTRICITY_MC = "history_cartElectricity";
    public const string DATABASE_HISTORY_ROTELECTRICITY_MC = "history_rotElectricity";
    public const string DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC = "history_suspensoidElectricity";
    public const string DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC = "history_bucketWheelElectricity";
    public const string DATABASE_HISTORY_CANTILEVER_Flow_MC = "history_cantileverFlow";
    public const string DATABASE_HISTORY_LOG1_MC = "history_logs";
    public const string DATABASE_HISTORY_LOG2_MC = "history_logs_two";
    public const string DATABASE_HISTORY_WARNING1_MC = "history_warning";
    public const string DATABASE_HISTORY_WARNING2_MC = "history_warning_two";
    public const string DATABASE_TASK_CONFIG = "task_config";
    #endregion

    #region login
    public const string DATA_USERNAME = "username";
    public const string DATA_PASSWORD = "password";
    public const string DATA_IS_ADMIN = "super_admin";
    // public const string DATA_INDEX = "Index";
    // public const string DATA_DEPARTMENT = "Department";
    // public const string DATA_JOB = "Job";
    // public const string DATA_DEFAULT_PASSWORD = "DefaultPassword";
    public const string DATA_NAME = "real_name";
    public const string DATA_USER_ID = "id";
    #endregion
    #region history_task_mc
    public const string DATA_TASK_ID = "task_id";
    public const string DATA_OPERATO_RSYSTEM = "operator_system";
    public const string DATA_TASK_CREATE_TIME = "task_create_time";
    public const string DATA_MACHINE = "machine";
    public const string DATA_TASK_TYPE = "task_type";
    public const string DATA_MATERIAL_RANGE_START = "material_range_start";
    public const string DATA_MATERIAL_RANGE_END = "material_range_end";
    public const string DATA_SIDE_SELECTION = "side_selection";
    public const string DATA_LEFT_RIGHT_RANGE_START = "left_right_range_start";
    public const string DATA_LEFT_RIGHT_RANGE_END = "left_right_range_end";
    public const string DATA_STEP_LENGTH = "step_length";
    public const string DATA_IS_TIMED = "is_timed";
    public const string DATA_TIMEDAT = "timed_at";
    public const string DATA_IS_QUANTIFIED = "is_quantified";
    public const string DATA_QUANTITY = "quantity";
    public const string DATA_OPERATOR = "operator";
    public const string DATA_TASK_STATE = "task_state";
    public const string DATA_TASK_TAKE_MATE_HIGH = "take_mate_high";
    public const string DATA_TASK_LAYER_HIGH = "layer_high";
    public const string DATA_TASK_AUTO_MODE = "auto_mode";
    public const string DATA_TASK_ANGLE_ENTRY_MODE = "angle_entry_mode";
    public const string DATA_TASK_ANGLE_ENTRY_VALUE = "angle_entry_value";
    public const string DATA_TASK_STATE2 = "state";
    public const string DATA_TASK_END_TIME = "task_end_time";
    public const string DATA_TASK_TURN_MODE = "turn_mode";
    public const string DATA_TASK_PILE_MODE = "pile_mode";
    public const string DATA_TASK_IS_CONTINUED = "is_task_continued";
    public const string DATA_TASK_PILE_MATE_HEIGH = "pile_mate_heigh";
    #endregion
    #region history_cartelectricity
    public const string DATA_HISTORY_CARTELECTRICITY_NAME = "name";
    public const string DATA_HISTORY_CARTELECTRICITY_TIME = "time";
    public const string DATA_HISTORY_CARTELECTRICITY_MACHINE = "machine";
    public const string DATA_HISTORY_CARTELECTRICITY_VALUE = "value";
    public const string DATA_HISTORY_CARTELECTRICITY_CREATOR = "creator";
    public const string DATA_HISTORY_CARTELECTRICITY_Create_date = "create_date";
    #endregion
    #region history_logs

    public const string DATA_HISTORY_LOGS_ID = "id";
    public const string DATA_HISTORY_LOGS_TIME = "time";
    public const string DATA_HISTORY_LOGS_INFO = "info";
    public const string DATA_HISTORY_LOGS_OPERATOR = "operator";
    public const string DATA_HISTORY_LOGS_CREATE_DATE = "create_date";
    public const string DATA_HISTORY_LOGS_CREATOR = "creator";
    #endregion
    #region history_warning
    public const string DATA_HISTORY_WARNING_TIME = "time";
    public const string DATA_HISTORY_WARNING_INFO = "info";
    public const string DATA_HISTORY_WARNING_OPERATOR = "operator";
    public const string DATA_HISTORY_WARNING_CREATE_DATE = "create_date";
    public const string DATA_HISTORY_WARNING_CREATOR = "creator";
    #endregion
    #region task_config
    public const string DATA_TASK_CONFIG_ID = "id";
    public const string DATA_TASK_CONFIG_HEAPDOS = "heap_dis";
    public const string DATA_TASK_CONFIG_MOVEMODEL = "move_model";
    public const string DATA_TASK_CONFIG_FETCHPILEDEPTH = "fetch_pile_depth";
    public const string DATA_TASK_CONFIG_FETCHVERTICALRANGEADD = "fetch_vertical_range_add";
    public const string DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB = "fetch_horizontal_range_sub";
    public const string DATA_TASK_CONFIG_REVERSALSETLEFT = "reversal_set_left";
    public const string DATA_TASK_CONFIG_REVERSALSETRIGHT = "reversal_set_right";
    public const string DATA_TASK_CONFIG_CREATOR = "creator";
    public const string DATA_TASK_CONFIG_CREATE_DATE = "create_date";
    #endregion
    #region
    public const string DATA_ACCOUNT_OPERATION_INDEX = "index";
    public const string DATA_ACCOUNT_OPERATION_TIME = "time";
    public const string DATA_ACCOUNT_OPERATION_OPERATOR = "operator";
    public const string DATA_ACCOUNT_OPERATION_DETAIL = "detail";
    #endregion

    #region plc variables
    public const string DATA_TIME_STAMP = "TimeStamp";
    #endregion

    #endregion
    #region
    public const string PROJECT_NAME = "华润电力(沈阳)斗轮机无人值守系统";
    #endregion
    
    #region
    public const string RC_SERVER_CONNECTION_FAIL_TIP = "服务器连接失败，请检查网络连接！ ";
    public const string TASK_SERVER_CONNECTION_FAIL_TIP = "服务器连接失败，请检查网络连接！ ";
    public const string SCA_SERVER_CONNECTION_FAIL_TIP = "服务器连接失败，请检查网络连接！ ";
    #endregion

    public const float InitDistance = 64.34f;//取料机差值
    public const float InitPosition_1 = 53.4f;
    public const float InitPosition_2 = 117.74f;
    public const float InitBucketWheelHeigh = 7.48f;
    public const string BucketWheelStackerReclaimerName = "1#斗轮堆取料机";
    public const string BucketWheelName = "2#斗轮取料机";
    public const string Version = "v0.3.20250122-r";
}

public enum Machine
{
    [Header("斗轮堆取料机")]
    BucketWheelStackerReclaimer,
    [Header("斗轮取料机")]
    BucketWheel,
    None,
}