namespace Commercial_Controller
{
    //Button on a floor or basement to go back to lobby
    public class FloorRequestButton
    {
        public int ID;
        public string status;
        public int buttonFloor;
        public string direction;
        public FloorRequestButton(int _ID,string _status,int _buttonFloor, string _direction)
        {
            this.ID = _ID;
            this.status = _status;
            this.buttonFloor = _buttonFloor;
            this.direction = _direction;
        }
    }
}