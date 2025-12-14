using ListPagination;

namespace Test;
[TestClass]
public sealed class PageListTest
{
    [TestMethod]
    public void Give_List_ReturnsPagedList()
    {
        List<int> testList = new([0, 1, 2]);
        PagedList<int> testPagedList = PagedList<int>.ToPagedList(testList.AsQueryable(), pageSize: 2, pageNumber: 2);
        Assert.AreEqual(3, testPagedList.TotalCount);
        Assert.AreEqual(2, testPagedList.CurrentPage);
        Assert.AreEqual(2, testPagedList.PageSize);
    }
}
