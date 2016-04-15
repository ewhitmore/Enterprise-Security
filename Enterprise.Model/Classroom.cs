using System.Collections.Generic;

namespace Enterprise.Model
{
    public class Classroom : EntityBase<Classroom>
    {
        public virtual string Name { get; set; }
        public virtual int Desks { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual IList<Student> Students { get; set; }
    }
}
