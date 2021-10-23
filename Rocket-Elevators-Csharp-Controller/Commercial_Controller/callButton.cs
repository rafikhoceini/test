namespace Rocket_Elevators_Csharp_Controller
{
    //Button on a floor or basement to go back to lobby
    public class CallButton
    {
        public int ID;
        public string status;
        public int floor;
        public string direction; 
        public CallButton(int _ID,string _status ,int _floor, string _direction)
        {
            this.ID = _ID;
            this.status = _status;
            this.floor = _floor;
            this.direction = _direction;
        }
    }
}