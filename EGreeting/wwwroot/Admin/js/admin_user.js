$(document).ready(function () {
    $('#table_user').DataTable({
        'columnDefs': [{
            'targets': [-1],
            'orderable': false,
        }],
        searching: true,
        iDisplayLength: 7,
        "ordering": false,
        lengthChange: false,
        aaSorting: [[0, "desc"]],
       
    });

    $(".change-active").on("click", function () {
        const userId = $(this).data("id");
        const url = `/admin/user/change-active/${userId}`;

        axios.get(url)
            .then(function (response) {
                if (response.data.code === 200) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: 'Successfully repaired',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: `${response.data.message}`,
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            return;

                        }
                    });
                }
            })
            .catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: `${response.data.message}`,
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            });
    });

    $(".remove-user").on("click", function () {
        const userId = $(this).data("id");
        const url = `/admin/user/delete-user/${userId}`;
        Swal.fire({
            title: 'Delete users',
            text: 'Are you sure you want to delete',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No!!!'
        }).then((result) => {
            if (result.isConfirmed) {
                axios.get(url)
                    .then(function (response) {
                        if (response.data.code === 200) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Success',
                                text: 'Deleted successfully',
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    window.location.reload();
                                }
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: `${response.data.message}`,
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    return;

                                }
                            });
                        }
                    })
                    .catch(function (error) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: `${response.data.message}`,
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                return;

                            }
                        });
                    });
            } else {
                return;
            }
        })

    });
    $('[data-bs-toggle="modal"]').on('click', function () {
        const userId = $(this).data('id');
        const userName = $(this).data('username');

        $('#name-user').text(userName);
        $('.add-money').data('id', userId);
    });
    $('.add-money').on('click', function () {
        const userId = $(this).data('id');
        const amount = $('#price').val();

        if (!amount || amount <= 0) {
            alert('Please enter a valid amount!');
            return;
        }

        const formData = new FormData();
        formData.append('id', userId);
        formData.append('price', amount);

        axios.post('/admin/update-money', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then(function (response) {
            if (response.data.code === 200) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Add Success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: `${response.data.message}`,
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            }
        })
            .catch(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: `An error occurred, please try again`,
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            });

    });
});