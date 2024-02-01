using System;

namespace TreeDesigner
{
    [Serializable]
    [NodeName("Sub")]
    [NodePath("Base/Value/Math/Sub")]
    public class SubNode : MathNode
    {
        protected override void OutputValue()
        {
            base.OutputValue();
            switch (m_InputValue1)
            {
                case FloatPropertyPort inputFloat:
                    (m_OutputValue as FloatPropertyPort).Value = inputFloat.Value - (m_InputValue2 as FloatPropertyPort).Value;
                    break;
            }
        }
    }
}