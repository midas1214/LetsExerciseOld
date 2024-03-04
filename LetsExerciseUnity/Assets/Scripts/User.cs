using SQLite4Unity3d;

public class User
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Age { get; set; }
    public int [] Weight { get; set; }
    public int Height { get; set; }
    public int [] BMI { get; set; }
    public string Gender { get; set; }

    public override string ToString()
    {
        return string.Format("[Person: Id={0}, Name={1},  Surname={2}, Age={3}]", Id, Age, Weight, Height);
    }
}
