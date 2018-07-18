using TurnBasedFeest.Actors;

namespace TurnBasedFeest.Attributes
{
    class GivenAttribute
    {
        public int expiration;
        public IAttribute attribute;
        public Actor receiver;

        public GivenAttribute(int expiration, IAttribute attribute, Actor receiver)
        {
            this.expiration = expiration;
            this.attribute = attribute;
            this.receiver = receiver;
        }
    }
}
