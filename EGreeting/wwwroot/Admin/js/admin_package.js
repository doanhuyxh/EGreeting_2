var vue_package = new Vue({
    el: "#vue_package",
    data: {
        IDPackage:0,
        Duration: "1 day",
        Decription: "",
        Price:0,
    },
    methods: {
        resetData() {
            IDPackage = 0
            Duration = "1 day"
            Decription = ""
            Price = 0

        },
        formatCurrency(amount) {
            const formatter = new Intl.NumberFormat('vi-VN', {
                style: 'currency',
                currency: 'VND'
            });

            return formatter.format(amount);
        },
        async addPackage() {
            try {
                $('#preloader').fadeIn();


                const formData = new FormData();
                formData.append('Duration', this.Duration);
                formData.append('Decription', this.Decription);
                formData.append('Price', this.Price);
               

                await axios.post('/admin/add-package', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) => {
                        $('#preloader').fadeOut();
                        if (response.data.code == 200) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Success',
                                text: 'Saved successfully',
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

                    }).finally(() => {
                        $('#preloader').fadeOut();
                    });


            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error has occurred',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            }
        },
        getItemsById(id) {
            axios.get(`/admin/get-package/${id}`)
                .then((response) => {
                    this.IDPackage = response.data.data.idPackage;
                    this.Price = response.data.data.price;
                    this.Decription = response.data.data.decription;
                    this.Duration = response.data.data.duration;
                   
                    return Promise.resolve();
                });
        },
        async editPackage() {
            try {
                $('#preloader').fadeIn();


                const formData = new FormData();
                formData.append('IDPackage', this.IDPackage);
                formData.append('Price', this.Price);
                formData.append('Decription', this.Decription);
                formData.append('Duration', this.Duration);
              

                await axios.post('/admin/edit-package', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) => {
                        $('#preloader').fadeOut();
                        if (response.data.code == 200) {
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

                    }).finally(() => {
                        $('#preloader').fadeOut();
                    });


            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error has occurred',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            }
        },
        async getItemsByIdDelete(id) {

            try {
                Swal.fire({
                    title: 'Delete package',
                    text: 'Are you sure you want to delete',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No!!!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#preloader').fadeIn();

                        const formData = new FormData();
                        formData.append('id', id);

                        axios.post('/admin/remove-package', formData, {
                            headers: {
                                'Content-Type': 'application/x-www-form-urlencoded'
                            }
                        }).then(response => {
                            if (response.data.code == 200) {
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

                        }).catch(error => {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: 'An error occurred, please try again',
                                confirmButtonText: 'OK'
                            });
                        }).finally(() => {
                            $('#preloader').fadeOut();
                        });
                    } else {
                        return;
                    }
                });

            }
            catch (error) {
                $('#preloader').fadeOut();

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred, please try again',
                    confirmButtonText: 'OK'
                });
                return;

            }
        },
    }
});