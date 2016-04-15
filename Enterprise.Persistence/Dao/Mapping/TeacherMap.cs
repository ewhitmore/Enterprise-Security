using Enterprise.Model;

namespace Enterprise.Persistence.Dao.Mapping
{
    internal class TeacherMap : EntityBaseMap<Teacher>
    {
        public TeacherMap()
        {
            Map(teacher => teacher.FullName);
            Map(teacher => teacher.Birthday);

            //http://stackoverflow.com/questions/6085568/how-to-do-a-fluent-nhibernate-one-to-one-mapping
            References(x => x.Classroom).Unique();
        }
    }
}
