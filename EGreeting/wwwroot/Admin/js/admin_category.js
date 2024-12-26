var vue_category = new Vue({
    el: "#vue_category",
    data: {
        IDCategoryCard: 0,      
        NameCategory: '',      
        ShortDecription: '',      
        IsComming: false,      
        IsOutstanding: false, 
        Slug: "",
        IsDefault: false, 

    },
    methods: {
        async addCategory() {
         
            try {
                $('#preloader').fadeIn();


                const formData = new FormData();
                formData.append('NameCategory', this.NameCategory);
                formData.append('ShortDecription', this.ShortDecription);
                formData.append('IsComming', this.IsComming);
                formData.append('IsOutstanding', this.IsOutstanding);
                formData.append('IsDefault', this.IsDefault);

                await axios.post('/admin/add-category', formData,
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
        resetData() {
            this.IDCategoryCard =0
            this.NameCategory = ''
            this.IsComming = false
            this.IsOutstanding = false
            this.Slug = ""
            this.IsDefault = false
        },
        getItemsById(id) {
            axios.get(`/admin/get-category/${id}`)
                .then((response) => {
                    this.IDCategoryCard = response.data.data.idCategoryCard;
                    this.NameCategory = response.data.data.nameCategory;
                    this.ShortDecription = response.data.data.shortDecription;
                    this.IsComming = response.data.data.isComming;
                    this.IsOutstanding = response.data.data.isOutstanding;
                    this.Slug = response.data.data.slug;
                    this.IsDefault = response.data.data.isDefault;

                    return Promise.resolve();
                });
        },
        async editCategory() {
            try {
                $('#preloader').fadeIn();


                const formData = new FormData();
                formData.append('IDCategoryCard', this.IDCategoryCard);
                formData.append('NameCategory', this.NameCategory);
                formData.append('ShortDecription', this.ShortDecription);
                formData.append('IsComming', this.IsComming);
                formData.append('IsOutstanding', this.IsOutstanding);
                formData.append('IsDefault', this.IsDefault);

                await axios.post('/admin/edit-category', formData,
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
                    title: 'Delete category',
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

                        axios.post('/admin/remove-category', formData, {
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