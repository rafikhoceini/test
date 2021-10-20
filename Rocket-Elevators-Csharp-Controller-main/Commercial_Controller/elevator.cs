using System.Threading;
using System.Collections.Generic;
using System;

namespace Commercial_Controller
{
    public class Elevator
    {

        public int floor;
        public string ID;
         public string status;
       public int amountOfFloors;
       public int currentFloor;
       public Door door;
       public string direction;
       public bool overweight;

        int screenDisplay;

       public List<int> floorRequestsList;

       public List<int> completedRequestsList;

        
       public bool doorIsObstructed;

        


        public Elevator(string _elevatorID,string _status, int _amountOfFloors,int _currentFloor )
        {
            this.ID = _elevatorID;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.currentFloor = _currentFloor;
            door = new Door(Int32.Parse(this.ID),"closed");
            floorRequestsList = new List<int>();
            completedRequestsList = new List<int>();
            this.direction = null;
            this.overweight = false;
        }
        public void move()
        {
            while(this.floorRequestsList.Count != 0 ){
                int destination = floorRequestsList[0];
                status = "moving";
                if(this.currentFloor < destination){
                    this.direction = "up";
                    this.sortFloorList();
                    while(this.currentFloor<destination){
                        this.currentFloor++;
                        this.screenDisplay = this.currentFloor;

                    }
                
                }
                else if(this.currentFloor>destination){
                    this.direction = "down";
                    this.sortFloorList();
                    while(this.currentFloor>destination){
                        this.currentFloor--;
                        this.screenDisplay = this.currentFloor;
                    }
                    
                }

                this.status = "stopped";
                this.operateDoors();
                completedRequestsList.Add(floorRequestsList[0]);
                this.floorRequestsList.RemoveAt(0);


            }
            this.status = "idle";
        }

        public void sortFloorList(){
            if(this.direction == "up"){
                this.floorRequestsList.Sort();
            }
            else{
                this.floorRequestsList.Reverse();
            }
        }

        public void operateDoors() {

             overweight = false;
             doorIsObstructed = false;
            this.door.status = "open";
            if (!this.overweight) {
                this.door.status = "closing";
                if (!doorIsObstructed) {
                    this.door.status = "closed";
                } else {
                    this.door.status = "opened";
                }

            } else {
                while(this.overweight) {
                    alarm();
                }
            }
        }

        public void addNewRequest(int requestedFloor){
            if(!this.floorRequestsList.Contains(requestedFloor)){
                this.floorRequestsList.Add(requestedFloor);

            }
            if(this.currentFloor<requestedFloor){
                this.direction = "up";
            }
            if(this.currentFloor>requestedFloor){
                this.direction = "down";
            }
        }
        
        
        
        
        
        public void alarm(){
            
        }
        
    }
}