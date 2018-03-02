// comment here 

function CheckSecuityConfig(IsSecurityApplied) {
    if (IsSecurityApplied === "False") {
        var div = $('#popup').html();
        dialog = bootbox.dialog({
            message: div,
            size: 'large',
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm',
                    callback: function (result) {
                        var form = dialog.find('#entityform');
                        form.bootstrapValidator({
                            message: 'This value is not valid',
                            feedbackIcons: {
                                valid: 'glyphicon glyphicon-ok',
                                invalid: 'glyphicon glyphicon-remove',
                                validating: 'glyphicon glyphicon-refresh'
                            },
                            fields: {
                                Password: {
                                    validators: {
                                        identical: {
                                            field: 'ConfirmPassword',
                                            message: 'The Password and its confirm are not the same'
                                        },
                                        notEmpty: {
                                            message: 'Password is required.'
                                        }
                                    }
                                },
                                ConfirmPassword: {
                                    validators: {
                                        identical: {
                                            field: 'Password',
                                            message: 'The Password and its confirm are not the same'
                                        },
                                        notEmpty: {
                                            message: 'Confirm Password is required.'
                                        }
                                    }
                                },
                                SecurityAnswer: {
                                    validators: {
                                        notEmpty: {
                                            message: 'Security Answer is required.'
                                        }
                                    }
                                },
                                LMSSecurityQuestionId: {
                                    validators: {
                                        callback: {
                                            message: 'Security Question is required.',
                                            callback: function (value, validator, $field) {
                                                /* Get the selected options */
                                                var options = validator.getFieldElements('LMSSecurityQuestionId').val();
                                                return (options !== '-1');
                                            }
                                        }
                                    }
                                },
                            }
                        });
                        var validator = form.data('bootstrapValidator');
                        validator.validate();

                        if (validator.isValid()) {
                            var Password = dialog.find('#Password').val();
                            var ConfirmPassword = dialog.find('#ConfirmPassword').val();
                            var LMSSecurityQuestionId = dialog.find('#LMSSecurityQuestionId').val();
                            var SecurityAnswer = dialog.find('#SecurityAnswer').val();
                            var HasQuestionId = $('#HasQuestionId').val();
                            var model = {
                                Password: Password,
                                ConfirmPassword: ConfirmPassword,
                                HasQuestionId: HasQuestionId,
                                LMSSecurityQuestionId: LMSSecurityQuestionId,
                                SecurityAnswer: SecurityAnswer,
                            };
                            $.ajax({
                                type: 'post',
                                url: GetRootDirectory() + '/admin/SecuityConfig',
                                data: model
                            })
                            .done(function (data) {
                                if (data && data.isSuccess) {
                                    dialog.modal('hide');
                                    location.href = GetRootDirectory() + '/Employee/EmployeeList';
                                }
                                else {
                                    dialog.modal('hide');
                                    bootbox.alert(data.message);
                                }
                            })
                            .fail(function (x, y, x) {
                                alert("error");
                            });
                        }
                        else
                            return false;
                    }
                }
            },
            onEscape: function () {
                this.modal('hide');
            }
        });
        dialog.find('#LMSSecurityQuestionId').selectpicker({ width: '27%' });
    }
    else
        location.href = GetRootDirectory() + '/Employee/EmployeeList';
}