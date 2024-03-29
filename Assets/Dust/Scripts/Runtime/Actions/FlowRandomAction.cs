﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dust
{
    [AddComponentMenu("Dust/Actions/Flow Random Action")]
    public class FlowRandomAction : FlowAction
    {
        [Serializable]
        public class Record
        {
            [SerializeField]
            private Action m_Action;
            public Action action
            {
                get => m_Action;
                set => m_Action = value;
            }

            [SerializeField]
            private float m_Weight = 0.5f;
            public float weight
            {
                get => m_Weight;
                set => m_Weight = Mathf.Clamp01(value);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        
        [SerializeField]
        private List<Record> m_Actions = new List<Record>();
        public List<Record> actions => m_Actions;
        
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        [SerializeField]
        private int m_Seed;
        public int seed
        {
            get => m_Seed;
            set
            {
                if (!IsAllowUpdateProperty()) return;

                if (m_Seed == value)
                    return;

                m_Seed = value;
                m_DuRandom = null;
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        private DuRandom m_DuRandom;
        private DuRandom duRandom => m_DuRandom ??= new DuRandom(seed);

        //--------------------------------------------------------------------------------------------------------------
        // Dust.Action lifecycle

        protected override void OnActionExecute()
        {
            if (actions.Count == 0)
                return;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Calculate Weight

            var totalWeight = actions.Where(Dust.IsNotNull).Sum(actionRecord => actionRecord.weight);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // If ALL are ZERO -> just call random with same weights

            if (DuMath.IsZero(totalWeight))
            {
                var actionRecord = actions[duRandom.Range(0, actions.Count)];
                
                if (Dust.IsNotNull(actionRecord) && Dust.IsNotNull(actionRecord.action))
                    actionRecord.action.Play();
                
                return;
            }
            
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Choose

            var randomWeight = duRandom.Range(0f, totalWeight);
            
            foreach (var actionRecord in actions.Where(Dust.IsNotNull))
            {
                if (randomWeight > actionRecord.weight)
                {
                    randomWeight -= actionRecord.weight;
                    continue;
                }

                if (Dust.IsNotNull(actionRecord.action))
                    actionRecord.action.Play();

                break;
            }
        }
    }
}
