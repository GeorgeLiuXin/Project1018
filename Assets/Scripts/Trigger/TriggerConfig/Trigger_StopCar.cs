using UnityEngine;

namespace Galaxy
{
    public class Trigger_StopCar : GalaxyTrigger
    {
        public override void OnTriggerEnterCallBack(GameObject target)
        {
            if (target == null)
                return;

            RoutineRunner.WaitForSeconds(2f, () =>
             {
                 GalaxyGameModule.GetGameManager<ChaseCarManager>().OnOutOfCar();
             });
        }
    }
}
