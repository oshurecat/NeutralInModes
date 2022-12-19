using NeutralInModes;

namespace NeutralInModes.Roles;

public class Bestfalsecharge
{
    public static void WrapUp()
    {
        if (ModeHandler.IsMode(ModeId.Default) && AmongUsClient.Instance.AmHost && !RoleClass.Bestfalsecharge.IsOnMeeting)
        {
            foreach (PlayerControl p in RoleClass.Bestfalsecharge.BestfalsechargePlayer)
            {
                p.RpcExiledUnchecked();
                p.RpcSetFinalStatus(FinalStatus.BestFalseChargesFalseCharge);
            }
            RoleClass.Bestfalsecharge.IsOnMeeting = true;
        }
    }
}
