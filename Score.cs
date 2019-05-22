
namespace Tetris
{

    public class Score
    {
        private string _name;
        private int _point;
        private Character _character;

        public Score() { }

        public Score(string name, int point, Character character) : this()
        {
            _name = name;
            _point = point;
            _character = character;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Point
        {
            get { return _point; }
        }

        public Character Character
        {
            get { return _character; }
        }
    }
}