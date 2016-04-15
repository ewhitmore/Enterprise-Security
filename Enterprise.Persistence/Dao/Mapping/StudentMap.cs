using Enterprise.Model;

namespace Enterprise.Persistence.Dao.Mapping
{
    public class StudentMap : EntityBaseMap<Student>
    {

        public StudentMap()
        {
            Map(student => student.FullName);
            Map(student => student.Birthday);

            References(student => student.Classroom).Cascade.SaveUpdate();
        }
    }
}
