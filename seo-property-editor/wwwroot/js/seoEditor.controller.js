(function () {
    'use strict';

    function seoEditorController($scope, $http) {
        var vm = this;
        vm.fieldNames = '';

        vm.generate = function () {
            if (!vm.fieldNames) {
                return;
            }
            var names = vm.fieldNames.split(',').map(function (n) { return n.trim(); });
            var fields = {};
            names.forEach(function(name){
                var value = $scope.$parent.$parent.content[name];
                if (value) {
                    fields[name] = value;
                }
            });
            $http.post('/umbraco/backoffice/SeoPropertyEditor/SeoContentGeneratorApi/Generate', { fields: fields })
                .then(function (res) {
                    $scope.model.value = res.data;
                });
        };
    }

    angular.module('umbraco').controller('seoEditorController', seoEditorController);
})();
