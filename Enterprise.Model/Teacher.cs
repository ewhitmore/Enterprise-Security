using System;

namespace Enterprise.Model
{
    public class Teacher : EntityBase<Teacher>
    {

        public virtual string FullName { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual Classroom Classroom { get; set; }

    }
}
