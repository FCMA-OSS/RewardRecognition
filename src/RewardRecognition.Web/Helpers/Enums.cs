using System;
using System.ComponentModel;

namespace FCMA.RewardRecognition.Web.Helpers
{
    public enum RewardStatus
    {
        PendingApproval = 1,    //P
        Approved = 2,           //A
        Denied = 3,             //D
        Withdrawn = 4,          //W
        Redeemed = 5            //R
    }
}