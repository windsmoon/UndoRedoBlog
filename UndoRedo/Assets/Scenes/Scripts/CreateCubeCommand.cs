using System;
using UnityEngine;

namespace Windsmoon
{
    public class CreateCubeCommand : Command
    {
        #region fields
        private RaycastHit raycastHit;
        private Action<RaycastHit> excuteAction;
        private Action<RaycastHit> undoAction;
        #endregion

        #region properties
        public CreateCubeCommand(Action<RaycastHit> excuteAction, Action<RaycastHit> undoAction, RaycastHit raycastHit)
        {
            this.excuteAction = excuteAction;
            this.undoAction = undoAction;
            this.raycastHit = raycastHit;
        }
        #endregion

        #region methods
        public override void Excute()
        {
            excuteAction(raycastHit);
        }

        public override void Undo()
        {
            undoAction(raycastHit);
        }

        public override string ToString()
        {
            return "CreateCubeCommand : " + raycastHit.collider.gameObject.name;
        }
        #endregion
    }
}
