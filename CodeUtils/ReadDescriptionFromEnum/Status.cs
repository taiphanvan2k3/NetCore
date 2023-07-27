using System.ComponentModel;

namespace CodeUtils.ReadDescriptionFromEnum
{
    public enum Status
    {
        [Description("Mới")]
        New,

        [Description("Đang diễn ra")]
        InProgress,

        [Description("Xác nhận")]
        Confirm,

        [Description("Hoàn thành")]
        Done
    }
}