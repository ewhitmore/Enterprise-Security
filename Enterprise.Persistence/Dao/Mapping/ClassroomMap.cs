
using Enterprise.Model;

namespace Enterprise.Persistence.Dao.Mapping
{
    internal class ClassroomMap : EntityBaseMap<Classroom>
    {
        public ClassroomMap()
        {
            Map(classroom => classroom.Name);
            Map(classroom => classroom.Desks);

            //http://stackoverflow.com/questions/6085568/how-to-do-a-fluent-nhibernate-one-to-one-mapping
            HasOne(classroom => classroom.Teacher).Cascade.All();
            HasMany(classroom => classroom.Students).Cascade.SaveUpdate();
        }
    }
}
