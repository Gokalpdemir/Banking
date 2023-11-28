namespace Bankacılık;

class Program
{
    static void Main(string[] args)
    {

        List<Customer> customers = new List<Customer>();
        List< UsedCredit> usedCredits = new List<UsedCredit>();
        List<Credit> credits = new List<Credit>();

        Credit personalLoan = new Credit("Personal Loan",1.1,12,100000);
        Credit autoLoan = new Credit("Auto Loan",0.8,18,500000);
        Credit mortgageLoan = new Credit("Mortgage Loan",0.6,24,1000000);
        credits.Add(personalLoan);
        credits.Add(autoLoan);
        credits.Add(mortgageLoan);


        while (true)
        {
            Console.Clear();
            Console.WriteLine("Demir Bank'a Hoş Geldiniz");
            Console.WriteLine("Müşteri olmak için bire basınız");
            Console.WriteLine("Müşteri Girişi yapmak için iki ye basınız");
            


                int operation = int.Parse(Console.ReadLine());

            switch (operation)
            {
                case 1:
                    Console.Write("İsim: ");
                    string name = Console.ReadLine();
                    Console.Write("Soyisim: ");
                    string surName = Console.ReadLine();
                    Console.Write("TC Kimlik No: ");
                    double tcNo =double.Parse( Console.ReadLine());
                    Console.Write("Açık Adres: ");
                    string address = Console.ReadLine();
                    Console.Write("Aylık Gelir: ");
                    int salary = int.Parse(Console.ReadLine());
                    
                    Customer customer1 = new Customer(name, surName, tcNo, address, salary);
                    Console.WriteLine("Müşteri Kaydı Başarılı!!");
                    customers.Add(customer1);

                    Thread.Sleep(3000);
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("Kullanıcı Girişi İçin TC No giriniz");
                    UsedCredit credit = new UsedCredit();
                    double userTcNo = double.Parse(Console.ReadLine());

                    bool login = false;
                    bool logout = true;

                    for (int i = 0; i < customers.Count; i++)
                    {
                        if (customers[i].TcNo == userTcNo)
                        {
                      

                            login = true;
                            logout = false;
                            Console.WriteLine($"Giriş Başarılı Hoş Geldin {customers[i].Name} {customers[i].SurName}");

                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Kredi çekmek için 1'e, kalan bakiye için 2'ye, Kredi detayı ve bakkiye için 3'e, Çıkış yapmak için 4 e basınız ");
                                int choose = int.Parse(Console.ReadLine());

                                if (choose == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Kredi Limitiniz : {customers[i].CreditLimit}");
                                    Console.WriteLine("kullanmak istediğiniz kredinin numarasını tuşlayınız");

                                    for (int j = 0; j < credits.Count; j++)
                                    {
                                        Console.WriteLine($" {j+1} : {credits[j].CreditName}");
                                    }
                                    int selectedCreditIndex = int.Parse(Console.ReadLine()) - 1; 
                                    if (selectedCreditIndex >= 0 && selectedCreditIndex < credits.Count)
                                    {
                                        Console.Clear();
                                        Credit selectedCredit = credits[selectedCreditIndex];
                                        Console.WriteLine($"Seçilen Kredi Bilgileri:");
                                        Console.WriteLine($"Kredi Adı: {selectedCredit.CreditName}");
                                        Console.WriteLine($"Faiz Oranı: {selectedCredit.InterestRate}");
                                        Console.WriteLine($"Maksimum Vadeli: {selectedCredit.MaxTerm} ay");
                                        Console.WriteLine($"Maksimum Kredi Miktarı: {selectedCredit.MaxParticipationAmount}");

                                        Console.WriteLine("Kredi vadesini giriniz:");
                                        int term = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Çekmek istediğiniz kredi tutarını giriniz:");
                                        int loanAmount = int.Parse(Console.ReadLine());
                                        bool creditResult= credit.CalculateCredit(customers[i].AvailableLimit, selectedCredit.Id, loanAmount, term ,usedCredits, customers[i].Id, customers,selectedCredit.CreditName,selectedCredit.InterestRate);

                                        if (creditResult)
                                            Console.WriteLine("Kredi işlemi başarılı");
                                        else
                                            Console.WriteLine("Kredi işlemi başarısız! Limitiniz yetersiz");

                                    }
                                    else
                                    {
                                        Console.WriteLine("Geçersiz kredi numarası.");
                                    }

                                    Thread.Sleep(3000);

                                }
                                else if (choose == 2)
                                {
                                    Console.WriteLine($"Kalan Kredi Limitiniz : " + customers[i].AvailableLimit);
                                    Thread.Sleep(3000);
                                }
                                else if (choose == 3)
                                {
                                    
                                    Console.WriteLine("Kullanılan Kredileriniz");
                                    List<UsedCredit> userCredit = credit.ShowCredits(usedCredits, customers[i].Id, customers);
                                    for (int k = 0; k < userCredit.Count; k++)
                                    {
                                            Console.WriteLine($"Kredi Adı : {userCredit[k].UsedCreditName}");
                                            Console.WriteLine($"Kredi Vadesi : {userCredit[k].Term}");
                                            Console.WriteLine($"Kredi Kullanım Tutarı : {userCredit[k].Amount}");
                                            Console.WriteLine("*******************************************************************");

                                    }
                                    Thread.Sleep(3000);
                                }
                                else if(choose==4){
                                    Console.WriteLine("Hoşçakal");
                                    logout=true;
                                }
                            } while (!logout);
                            
                        }
                    }

                    if (!login)
                    {
                        Console.WriteLine("Giriş Başarısız!");
                    }

                    Thread.Sleep(3000);
                    break;
            }         

        }

    }
    
}
class Customer
{
    public Guid Id { get; } 
    public string Name { get; set; }
    public string SurName { get; set; }
    public double TcNo { get; set; }
    public string Address { get; set; }
    public int Salary { get;set; }
    public int CreditLimit { get; set; }
    public Decimal AvailableLimit { get; set; }
    public Customer()
    {

    }
    public Customer(string name, string surName, double tcNo, string address, int salary)
    {
       this.Id = Guid.NewGuid();
        this.Name = name;
        this.SurName = surName;
        this.TcNo = tcNo;
        this.Address = address;
        this.Salary = salary;
        this.CreditLimit = CalculateCredit(tcNo, salary);
        this.AvailableLimit = this.CreditLimit;

    }
    public int CalculateCredit(double TcNo, int Salary)
    {
        int toplam = 0;
        string limit = TcNo.ToString();
        for (int i = 0; i < limit.Length; i++)
        {
            toplam += int.Parse(limit[i].ToString());
        }
        return toplam * 12 * Salary;
    }
   
}
class UsedCredit
{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CreditId { get; set; }
    public Guid CustomerId { get; set; }
    public int Term { get; set; }
    public string UsedCreditName { get; set; }
    public int Amount { get; set; }


    public bool CalculateCredit(decimal limit,Guid creditId ,int amount,int term ,List<UsedCredit> _credits,Guid id,List<Customer>_customers,string creditName,double rate)
    {
        bool limitIsOkey = false;
            if (limit >= amount)
            {
            limitIsOkey = true;
            UsedCredit newCredit = new UsedCredit
            {
                Id=Id,
                CreditId=creditId,
                CustomerId = id,
                UsedCreditName=creditName,
                Term=term,
                Amount = amount,
                    
                };
                _credits.Add(newCredit);
              foreach (var customer in _customers)
              {
                if (customer.Id == id)
                    customer.AvailableLimit -= Convert.ToDecimal((amount * rate)+amount);
                
              }
              
        }
        return limitIsOkey;
        
      }

    public int  CalculateRemaininCredit(List<Customer>_customers ,double tcNo)
    {
        int creditLimit = 0;
        foreach (var customer in _customers)
        {
            if (customer.TcNo == tcNo)
            {
                creditLimit =customer.CreditLimit;
            }

        }
        return creditLimit;
    }

    public List<UsedCredit> ShowCredits(List<UsedCredit> _credits, Guid id, List<Customer> _customers)
    {
       List<UsedCredit> userCredits = new List<UsedCredit>();

        for (int i = 0; i < _credits.Count; i++)
        {
            if (_credits[i].CustomerId == id)
            {
                userCredits.Add(_credits[i]);
            }
        }
               
        return userCredits;
    }


}
class Credit
{
    public Guid Id{get; set;}
    public string CreditName{get; set;}
    public double InterestRate{get; set;}
    public int MaxTerm {get; set; }
    public double MaxParticipationAmount { get; set; }

   public Credit(string creditName,double ınterestRate,int maxTerm,double maxParticipationAmount)
    {
        this.Id = Guid.NewGuid();
        this.CreditName = creditName;
        this.InterestRate = ınterestRate;
        this.MaxTerm = maxTerm;
        this.MaxParticipationAmount = maxParticipationAmount;
    }

    public Credit()
    {
        
    }
}

