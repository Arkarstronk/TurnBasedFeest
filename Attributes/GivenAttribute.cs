using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Attributes
{
    class GivenAttribute
    {
        public int Expiration;
        public IAttribute attribute;
        public Actor receiver;

        public GivenAttribute(int expiration, IAttribute attribute, Actor receiver)
        {
            this.Expiration = expiration;
            this.attribute = attribute;
            this.receiver = receiver;
        }
    }
}
