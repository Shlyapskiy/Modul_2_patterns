using System;

interface IInternetState
{
    void DeductMonthlyFee();
    void Recharge(double amount);
    void ConnectToNetwork();
}

class ActiveState : IInternetState
{
    private InternetAccount account;

    public ActiveState(InternetAccount acc)
    {
        account = acc;
    }

    public void DeductMonthlyFee()
    {
        if (account.Balance >= account.MonthlyFee)
        {
            account.Balance -= account.MonthlyFee;
            Console.WriteLine($"Абонплата списана. Поточний баланс: {account.Balance} грн.");
        }
        else
        {
            account.ChangeState(new ArrearsState(account));
        }
    }

    public void Recharge(double amount)
    {
        account.Balance += amount;
        Console.WriteLine($"Рахунок поповнено на {amount} грн. Поточний баланс: {account.Balance} грн.");
    }

    public void ConnectToNetwork()
    {
        Console.WriteLine($"Швидкiсть пiдключення {account.MaxSpeed} Мбiт/сек.");
    }
}

class ArrearsState : IInternetState
{
    private InternetAccount account;

    public ArrearsState(InternetAccount acc)
    {
        account = acc;
    }

    public void DeductMonthlyFee()
    {
        Console.WriteLine("Заборгованiсть iснує. Швидкiсть доступу знижено до 0.");
        account.ChangeSpeed(0);
    }

    public void Recharge(double amount)
    {
        account.Balance += amount;
        if (account.Balance >= 0)
        {
            Console.WriteLine("Заборгованiсть погашена. Швидкiсть доступу вiдновлено.");
            account.ChangeState(new ActiveState(account));
        }
    }

    public void ConnectToNetwork()
    {
        Console.WriteLine("Швидкiсть пiдключення 0 Мбiт/сек.");
    }
}

class InternetAccount
{
    public double Balance { get; set; }
    public double MonthlyFee { get; set; }
    public double MaxSpeed { get; set; }

    private IInternetState currentState;

    public InternetAccount(double balance, double monthlyFee, double maxSpeed)
    {
        Balance = balance;
        MonthlyFee = monthlyFee;
        MaxSpeed = maxSpeed;
        currentState = new ActiveState(this);
    }

    public void ChangeState(IInternetState newState)
    {
        currentState = newState;
    }

    public void ChangeSpeed(double speed)
    {
        MaxSpeed = speed;
    }

    public void DeductMonthlyFee()
    {
        currentState.DeductMonthlyFee();
    }

    public void Recharge(double amount)
    {
        currentState.Recharge(amount);
    }

    public void ConnectToNetwork()
    {
        currentState.ConnectToNetwork();
    }
}

class Program
{
    static void Main()
    {
        InternetAccount account = new InternetAccount(balance: 50, monthlyFee: 20, maxSpeed: 100);
        account.ConnectToNetwork();

        account.DeductMonthlyFee();
        account.ConnectToNetwork();

        account.Recharge(30);
        account.DeductMonthlyFee();
        account.ConnectToNetwork();

        account.Recharge(10);
        account.ConnectToNetwork();
    }
}
/*  Для  вирішення даної задачі я скористався шаблоном "Стан".
 Цей шаблон дозволяє об'єкту змінювати свою поведінку при зміні внутрішнього стану.
У даному випадку, стани можуть бути "Звичайний стан" і "Стан заборгованості", 
які визначатимуть поведінку об'єкта класу рахунки абонента.
*/
