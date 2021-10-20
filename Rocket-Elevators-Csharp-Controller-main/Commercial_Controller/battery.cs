using System;
using System.Collections.Generic;

namespace Commercial_Controller
{
    public class Battery
    {
        public int columnID = 1;
        public int elevatorID = 1;
        public int floorRequestButtonID = 1;
        public int callButtonID = 1;

        public int ID;
        public string status;
        public List<Column> columnsList;
        public List<FloorRequestButton> floorRequestsButtonsList;

        public Battery(int _ID, int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _ID;
            this.status = "online";
            columnsList = new List<Column>();
            floorRequestsButtonsList = new List<FloorRequestButton>();

            if (_amountOfBasements > 0){
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns--;
            }

            this.createFloorRequestButtons(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfBasements, _amountOfElevatorPerColumn);
        }

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn) {
            
            List<int> servedFloors = new List<int>();
            int floor = -1;
            
            for (int i = 0; i < _amountOfBasements; i++)
            {
                servedFloors.Add(floor);
                floor--;      
            }
            
            var column = new Column(columnID.ToString(), "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloors, true);
            columnsList.Add(column);
            columnID++;
        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn) {
            
            int amountOfFloorsPerColumn = (int)Math.Ceiling(Convert.ToDecimal(_amountOfFloors)/_amountOfColumns);
            int floor = 1;

            for (int i = 0; i < _amountOfColumns; i++)
            {
                List<int> servedFloors = new List<int>();

                for (int x = 0; x < amountOfFloorsPerColumn; x++)
                {
                    if (floor <= _amountOfFloors) {

                        servedFloors.Add(floor);
                        floor++;
                    }
                }
                    Column column = new Column(columnID.ToString(), "online", _amountOfFloors, _amountOfElevatorPerColumn, servedFloors, false);
                    columnsList.Add(column);
                    columnID++;
            }

            
        }

        public void createFloorRequestButtons(int _amountOfFloors) {

            int buttonFloor = 1;
            
            for (int i = 0; i < _amountOfFloors; i++)
            {
                var floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Up");
                this.floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor--;
                floorRequestButtonID++;
            }
        }

        public void createBasementFloorRequestButtons(int _amountOfBasements) {

            int buttonFloor = -1;

            for (int i = 0; i < _amountOfBasements; i++)
            {
                var floorRequestButton = new FloorRequestButton(floorRequestButtonID, "OFF", buttonFloor, "Down");
                this.floorRequestsButtonsList.Add(floorRequestButton);
                buttonFloor--;
                floorRequestButtonID++;
            }

        }
        
        public Column findBestColumn(int _requestedFloor)
        {
            foreach (Column column in this.columnsList)
            {
                if (column.servedFloors.Contains(_requestedFloor)) {

                    return column;
                }
            }
            return null;
        }
        //Simulate when a user press a button at the lobby
        public (Column, Elevator) assignElevator(int _requestedFloor, string _direction)
        {
            Column column = this.findBestColumn(_requestedFloor);
            Elevator elevator = column.findElevator(1, _direction);
            elevator.addNewRequest(1);
            elevator.move();

            elevator.addNewRequest(_requestedFloor);
            elevator.move();

            elevator.completedRequestsList.Add(_requestedFloor);
            return (column, elevator);
        }
    }
}

