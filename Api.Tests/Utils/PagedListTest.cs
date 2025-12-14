using ListPagination;

namespace Test;

[TestClass]
public sealed class PageListTest
{
    [TestMethod]
    public void Constructor_ShouldInitializeProperties()
    {
        var items = new List<int> { 1, 2, 3 };
        var paged = new PagedList<int>(items, count: 10, pageSize: 3, pageNumber: 2);

        Assert.AreEqual(10, paged.TotalCount);
        Assert.AreEqual(3, paged.PageSize);
        Assert.AreEqual(2, paged.CurrentPage);
        Assert.AreEqual(4, paged.TotalPages);
        Assert.IsTrue(paged.HasPrevious);
        Assert.IsTrue(paged.HasNext);
    }

    [TestMethod]
    public void Give_List_ReturnsPagedList()
    {
        List<int> testList = new([0, 1, 2]);
        PagedList<int> testPagedList = PagedList<int>.ToPagedList(
            testList.AsQueryable(),
            pageSize: 2,
            pageNumber: 2
        );
        Assert.AreEqual(testList.Count, testPagedList.TotalCount);
        Assert.AreEqual(2, testPagedList.CurrentPage);
        Assert.AreEqual(2, testPagedList.PageSize);
    }
}
