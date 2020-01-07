define(["doh/runner"], function (doh) {
    doh.register("MyTests", [
        function assertTrueTest() {
            doh.assertTrue(true);
            doh.assertTrue(1);
            doh.assertTrue(!false);
        }
    ]);

    doh.register("Test edit state", [
        {
            name: 'should test whether the edit state was entered successfully',
            runTest: function () {
                doh.assertTrue(true);
            }
        }
    ]);
});
