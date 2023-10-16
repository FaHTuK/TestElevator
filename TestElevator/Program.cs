
class Elevator
{
    public int CurrentFloor { get; set; } //Текущий этаж, на котором в данный момент находится лифт
    public string Direction { get; set; } //Направление Up/Down
}

class Passenger
{
    public int SourceFloor { get; set; } //Исходный этаж
    public int DestinationFloor { get; set; } //Этаж назначения
    public bool InElevator { get; set; } //Находится ли человек в лифте (если false, то он на этаже) 
}

class FloorButton
{
    public int FloorNumber { get; set; } //Номер этажа
    public bool IsPressed { get; set; } //Нажал ли человек 
}

class ElevatorController
{
    public List<Passenger> passengers;
    public List<FloorButton> floorButtons;
    public Elevator elevator;

    public ElevatorController()
    {
        passengers = new List<Passenger>();
        floorButtons = new List<FloorButton>();
        elevator = new Elevator();
    }

    //Человек вызывает лифт
    public void PressFloorButton(int floorNumber)
    {
        FloorButton button = floorButtons.Find(b => b.FloorNumber == floorNumber);
        if (button != null)
        {
            button.IsPressed = true;
        }

        passengers.Add(new Passenger { SourceFloor = floorNumber});
    }

    public void Update()
    {
        if (elevator.Direction == "Up")
        {
            if (elevator.CurrentFloor < 9)
            {
                elevator.CurrentFloor++;
            }
            else
            {
                elevator.Direction = "Down";
            }
        }
        else if (elevator.Direction == "Down")
        {
            if (elevator.CurrentFloor > 1)
            {
                elevator.CurrentFloor--;
            }
            else
            {
                elevator.Direction = "Up";
            }
        }

        // Если лифт находится на этаже, на котором есть пассажиры
        if (passengers.Exists(p => p.SourceFloor == elevator.CurrentFloor && p.InElevator == false))
        {
            //Производим посадку
            List<Passenger> boardingPassengers = passengers.FindAll(p => p.SourceFloor == elevator.CurrentFloor);
            foreach (Passenger passenger in boardingPassengers)
            {
                // Пассажир нажимает случайную кнопку этажа внутри лифта
                int destinationFloor = GetRandomFloor();
                passenger.DestinationFloor = destinationFloor;
                passenger.InElevator = true; //Обозначаем, что пассажир в лифте и на этаже его уже нет
            }

            //Вывод в консоль. Сколько село человек в лифт и какие этажи выбрали в качестве пункта назначения
            Console.WriteLine($"\uD83D\uDEEB {boardingPassengers.Count} пассажиров нашлись на этаже {elevator.CurrentFloor} и сели в лифт.");
            foreach (Passenger passenger in boardingPassengers)
            {
                Console.WriteLine($"\uD83D\uDECD Пассажир: Исходный этаж: {passenger.SourceFloor}, Этаж назначения: {passenger.DestinationFloor}");
            }
        }

        //Проверяем, есть ли пассажиры, которым необходимо выйти на данном этаже и выводим их из лифта
        List<Passenger> exitingPassengers = passengers.FindAll(p => p.DestinationFloor == elevator.CurrentFloor);
        foreach (Passenger passenger in exitingPassengers)
        {
            passengers.Remove(passenger);            
            passengers.Add(new Passenger { SourceFloor = elevator.CurrentFloor, InElevator = false });
        }
        Console.WriteLine($"\uD83D\uDEEB {exitingPassengers.Count} пассажиров вышли на этаже {elevator.CurrentFloor}");
    }

    //Рандомайзер для этажей
    public int GetRandomFloor()
    {
        Random random = new Random();
        int randomFloor = random.Next(1, 9);
        return randomFloor;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ElevatorController controller = new ElevatorController();

        // Создаем кнопки на каждом этаже
        for (int i = 1; i <= 9; i++)
        {
            controller.floorButtons.Add(new FloorButton { FloorNumber = i, IsPressed = false });
        }

        // Симулируем нажатие кнопок на разных этажах (по одному человеку на этажах 1,2,3)
        // Можно рандомно, подставив "controller.GetRandomFloor()"
        controller.PressFloorButton(1);
        controller.PressFloorButton(2);
        controller.PressFloorButton(3);

        controller.elevator.Direction = "Up";

        // Основной цикл программы
        while (true)
        {
            // Обновляем состояние лифта
            controller.Update();

            // Выводим информацию о текущем состоянии лифта
            Console.WriteLine($"Tекущий этаж: {controller.elevator.CurrentFloor} | Направление: {controller.elevator.Direction}");

            // Задержка в 1 секунду перед следующим обновлением
            Thread.Sleep(1000);
        }
    }
}
