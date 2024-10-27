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
    public const string DATABASE_LOGIN_TABLE = "login";
    public const string DATABASE_ACCOUNTOPERATION_TABLE = "accountoperation";
    public const string DATABASE_VARIABLE_TABLE = "plcvariables";
    public const string DATABASE_HISTORY_TASK_MC = "history_task_mc";
    public const string DATABASE_HISTORY_CARTELECTRICITY_MC = "history_cartElectricity";
    public const string DATABASE_HISTORY_ROTELECTRICITY_MC = "history_rotElectricity";
    public const string DATABASE_HISTORY_SUSPENSOID_ELECTRICITY_MC = "history_suspensoidElectricity";
    public const string DATABASE_HISTORY_BUCKETWHEEL_ELECTRICITY_MC = "history_bucketWheelElectricity";
    public const string DATABASE_HISTORY_CANTILEVER_Flow_MC = "history_cantileverFlow";
    public const string DATABASE_HISTORY_LOG1_MC = "history_logs";
    public const string DATABASE_HISTORY_LOG2_MC = "history_logs_2";
    public const string DATABASE_HISTORY_WARNING1_MC = "history_warning";
    public const string DATABASE_HISTORY_WARNING2_MC = "history_warning_2";
    public const string DATABASE_TASK_CONFIG = "task_config";
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
    public const string DATA_TASK_TAKE_MATE_HIGH = "TakeMateHigh";
    public const string DATA_TASK_LAYER_HIGH = "LayerHigh";
    #endregion
    #region history_cartelectricity
    public const string DATA_HISTORY_CARTELECTRICITY_NAME = "Name";
    public const string DATA_HISTORY_CARTELECTRICITY_TIME = "Time";
    public const string DATA_HISTORY_CARTELECTRICITY_MACHINE = "Machine";
    public const string DATA_HISTORY_CARTELECTRICITY_VALUE = "Value";
    #endregion
    #region history_logs
    public const string DATA_HISTORY_LOGS_TIME = "time";
    public const string DATA_HISTORY_LOGS_INFO = "info";
    public const string DATA_HISTORY_LOGS_OPERATOR = "operator";
    #endregion
    #region history_warning
    public const string DATA_HISTORY_WARNING_TIME = "time";
    public const string DATA_HISTORY_WARNING_INFO = "info";
    public const string DATA_HISTORY_WARNING_OPERATOR = "operator";
    #endregion
    #region task_config
    public const string DATA_TASK_CONFIG_ID = "ID";
    public const string DATA_TASK_CONFIG_HEAPDOS = "HeapDis";
    public const string DATA_TASK_CONFIG_MOVEMODEL = "MoveModel";
    public const string DATA_TASK_CONFIG_FETCHPILEDEPTH = "FetchPileDepth";
    public const string DATA_TASK_CONFIG_FETCHVERTICALRANGEADD = "FetchVerticalRangeAdd";
    public const string DATA_TASK_CONFIG_FETCHORIZONTALTANGESUB = "FetchHorizontalRangeSub";
    #endregion
    #region
    public const string DATA_ACCOUNT_OPERATION_INDEX = "Index";
    public const string DATA_ACCOUNT_OPERATION_TIME = "Time";
    public const string DATA_ACCOUNT_OPERATION_OPERATOR = "Operator";
    public const string DATA_ACCOUNT_OPERATION_DETAIL = "Detail";
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
}

public enum Machine
{
    [Header("斗轮堆取料机")]
    BucketWheelStackerReclaimer,
    [Header("斗轮取料机")]
    BucketWheel,
    None,
}