namespace _Project.Scripts.DomainObjects
{
    public struct AngleDefinition
    {
        public float expected;
        public float lowerTolerance;
        public float higherTolerance;
        public float threshold;

        public AngleDefinition(float expected, float lowerTolerance = 0f, float higherTolerance = 0f, float threshold = 0f)
        {
            this.expected = expected;
            this.lowerTolerance = lowerTolerance;
            this.higherTolerance = higherTolerance;
            this.threshold = threshold;
        }
    }
}