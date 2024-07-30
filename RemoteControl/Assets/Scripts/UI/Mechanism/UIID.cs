public partial class UIID
{
    public static readonly UIID MainPanel = new UIID("MainPanel",typeof(MainPanelView),typeof(MainPanelCtr),Layer.UILayer);
    public static readonly UIID LoginPanel = new UIID("LoginPanel",typeof(LoginPanelView),typeof(LoginPanelCtr),Layer.UILayer);
    public static readonly UIID TopPanel = new UIID("TopPanel",typeof(TopPanelView),typeof(TopPanelCtr),Layer.UITopLayer);
    public static readonly UIID SettingPanel = new UIID("SettingPanel",typeof(SettingPanelView),typeof(SettingPanelCtr),Layer.UILayer);
    public static readonly UIID ConfirmPanel = new UIID("ConfirmPanel",typeof(ConfirmPanelView),typeof(ConfirmPanelCtr),Layer.UILayer);
    public static readonly UIID HistoryPanel = new UIID("HistoryWarningPanel",typeof(HistoryPanelView),typeof(HistoryPanelCtr),Layer.UILayer);
    public static readonly UIID StatusParaeterPanel = new UIID("StatusParameterPanel", typeof(StatusParameterPanelView), typeof(StatusParameterPanelCtr), Layer.UILayer);
}