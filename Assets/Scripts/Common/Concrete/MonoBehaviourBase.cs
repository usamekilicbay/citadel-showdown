using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CitadelShowdown.DI
{
    public class MonoBehaviourBase : MonoBehaviour
    {
        protected CoreLoopFacade coreLoopFacade;

        [Inject]
        public virtual void Construct(CoreLoopFacade coreLoopFacade)
        {
            this.coreLoopFacade = coreLoopFacade;
        }
    }
}
