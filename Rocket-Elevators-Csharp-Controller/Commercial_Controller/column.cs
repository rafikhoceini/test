using System;
using System.Collections.Generic;

namespace Rocket_Elevators_Csharp_Controller
{
    public class Column
    {
       public int callButtonID =1; 
       public int elevatorID = 1;
       
        public string ID;
        public string status;
         public List<int> servedFloors; 
        public bool isBasement;
        public int amountOfElevator;
        public int amountOfFloors;
       public List<int> servedFloorsList;
         public List<Elevator> elevatorsList;
        public List<CallButton> callButtonsList;
        public Column(
            string _ID,
            string _status,
            int _amountOfFloors,
            int _amountOfElevator,
            List<int> _servedFloors,
            bool _isBasement)
        {
            this.ID = _ID;
            this.status = _status;
            servedFloors =_servedFloors;
            
            elevatorsList = new List<Elevator>();
            callButtonsList = new List<CallButton>();
            this.amountOfElevator = _amountOfElevator;
            this.amountOfFloors = _amountOfFloors;
            
            this.createElevators(_amountOfFloors,_amountOfElevator);
            this.createCallButtons(_amountOfFloors,_isBasement);
        }

// method create callButton and adds them to list:  c.callButtonsList
        public void createCallButtons(int _amountOfFloors,bool _isBasement){
            if(_isBasement){
                int buttonFloor = -1;
                for(int i = 0; i<_amountOfFloors; i++){
                    CallButton callButton = new CallButton(callButtonID,"OFF",buttonFloor,"Up");
                    this.callButtonsList.Add(callButton);
                    buttonFloor--;
                    callButtonID++;

                }
            }
            else{
                int buttonFloor = 1;
                for(int i = 0; i<_amountOfFloors; i++){
                    CallButton callButton = new CallButton(callButtonID,"OFF",buttonFloor,"Down");
                    this.callButtonsList.Add(callButton);
                    buttonFloor++;
                    callButtonID++;
                }
            }
        }
// method that creates elevator and add them to list called elevatorsList

        public void createElevators(int _amountOfFloors,int _amountOfElevators){
            for(int i = 0; i<_amountOfElevators; i++){
                Elevator elevator = new Elevator(elevatorID.ToString(),"idle",_amountOfFloors,1);
                this.elevatorsList.Add(elevator);
                elevatorID++;
            }
        }
            //Simulate when a user press a button on a floor to go back to the first floor
        public Elevator requestElevator(int _requestedFloor,string direction){ // i added on my own int _requestedFloor
            Elevator elevator = this.findElevator(_requestedFloor, direction);
            elevator.addNewRequest(_requestedFloor);
            elevator.move();
            elevator.addNewRequest(1);
            elevator.move();
            elevator.completedRequestsList.Add(_requestedFloor);
            return elevator;
        }

        // method that gives score depending on the situation an elevator is in

        public Elevator findElevator(int requestedFloor,string requestedDirection){
            Elevator bestElevator = null;
            int bestScore = 6;
            int referenceGap = 10000000;
            Tuple<Elevator,int,int> bestElevatorInformations;

            if(requestedFloor == 1 ){
                foreach(Elevator elevator in this.elevatorsList){
                    if(1 == elevator.currentFloor && elevator.status == "stopped"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(1,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(1 == elevator.currentFloor && elevator.status == "idle"){
                         bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(1>elevator.currentFloor && elevator.direction == "up"){
                         bestElevatorInformations = this.checkIfElevatorIsBetter(3,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(1<elevator.currentFloor && elevator.direction == "down" ){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(3,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else{
                         bestElevatorInformations = this.checkIfElevatorIsBetter(5,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }

                    bestElevator = bestElevatorInformations.Item1;
                    bestScore = bestElevatorInformations.Item2;
                    referenceGap = bestElevatorInformations.Item3;
                }
            }
            else{
                foreach(Elevator elevator in elevatorsList){
                    if(requestedFloor == elevator.currentFloor && elevator.status == "stopped" && requestedDirection == elevator.direction){
                          bestElevatorInformations = this.checkIfElevatorIsBetter(1,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(requestedFloor > elevator.currentFloor && elevator.direction == "up" && requestedDirection == "up"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(requestedFloor < elevator.currentFloor && elevator.direction == "down" && requestedDirection == "down"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(2,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else if(elevator.status == "idle"){
                        bestElevatorInformations = this.checkIfElevatorIsBetter(4,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                    else{
                        bestElevatorInformations = this.checkIfElevatorIsBetter(5,elevator,bestScore,referenceGap,bestElevator,requestedFloor);
                    }
                   
                    bestElevator = bestElevatorInformations.Item1;
                    bestScore = bestElevatorInformations.Item2;
                    referenceGap = bestElevatorInformations.Item3;
                
                }
                

            }

            return bestElevator;





        }
// method that gets all the scores from findElevator method and gets the best score ( gets the best elevator)

        public Tuple<Elevator,int,int> checkIfElevatorIsBetter(int scoreToCheck,Elevator newElevator, int bestScore,int referenceGap,Elevator bestElevator,int floor){
            if(scoreToCheck<bestScore){
               bestScore = scoreToCheck;
               bestElevator = newElevator;
               referenceGap = Math.Abs(newElevator.currentFloor - floor);

            }
            else if(bestScore == scoreToCheck){
               int gap = Math.Abs(newElevator.currentFloor - floor);
               if(referenceGap>gap){
                   bestElevator = newElevator;
                   referenceGap = gap;

               }
            }
            Tuple<Elevator,int,int> bestElevatorInformations = new Tuple<Elevator,int,int>(bestElevator,bestScore,referenceGap);
            return bestElevatorInformations;
        }
        
        
        
        
        

    }
}



