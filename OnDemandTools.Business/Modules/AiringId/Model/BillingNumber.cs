namespace OnDemandTools.Business.Modules.AiringId.Model
{
    public class BillingNumber
    {
        public int Lower { get; set; }
        public int Current { get; set; }
        public int Upper { get; set; }

        public void Increment()
        {
            Current++;

            if (Current < Lower || Current > Upper)
                Current = Lower;
        }
    }
}