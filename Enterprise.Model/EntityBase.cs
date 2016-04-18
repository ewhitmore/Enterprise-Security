using System;

namespace Enterprise.Model
{
    public abstract class EntityBase<T> where T : EntityBase<T>
    {
        public virtual int Id { get; set; }
        public virtual bool IsDeleted { get; set; } = false;
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedAt { get; set; }

        #region Methods

        private int? _oldHashCode;

        public override bool Equals(object obj)
        {
            var other = obj as T;
            if (other == null) return false;

            var thisIsNew = Equals(Id, 0);
            var otherIsNew = Equals(other.Id, 0);

            if (thisIsNew && otherIsNew)
                return ReferenceEquals(this, other);

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            // once we have a hashcode we'll never change it
            if (_oldHashCode.HasValue)
            {
                return _oldHashCode.Value;
            }
            // when this instance is new we use the base hash code
            // and remember it, so an instance can NEVER change its
            // hash code.
            var thisIsNew = Equals(Id, 0);

            if (thisIsNew)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<T> lhs, EntityBase<T> rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(EntityBase<T> lhs, EntityBase<T> rhs)
        {
            return !Equals(lhs, rhs);
        }

        #endregion
    }
}
