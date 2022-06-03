{{~ if Bag.PagesFolder; pagesNamespace = Bag.PagesFolder + "."; end ~}}
using System.Threading.Tasks;

namespace {{ ProjectInfo.FullName }}.Web.Pages.{{ pagesNamespace }}{{ EntityInfo.RelativeNamespace }}
{
    public class IndexModel : {{ ProjectInfo.Name }}PageModel
    {
        public virtual async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}
