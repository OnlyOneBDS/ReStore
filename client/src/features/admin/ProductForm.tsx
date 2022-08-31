import { yupResolver } from "@hookform/resolvers/yup";
import { LoadingButton } from "@mui/lab";
import { Box, Button, Grid, Paper, Typography } from "@mui/material";
import { useEffect } from "react";
import { FieldValues, useForm } from "react-hook-form";
import agent from "../../app/api/agent";

import AppDropZone from "../../app/components/AppDropZone";
import AppSelectList from "../../app/components/AppSelectList";
import AppTextInput from "../../app/components/AppTextInput";
import useProducts from "../../app/hooks/useProducts";
import { Product } from "../../app/models/product";
import { useAppDispatch } from "../../app/store/configureStore";
import { setProduct } from "../catalog/catalogSlice";
import { validationSchema } from "./productValidation";

interface Props {
  product?: Product;
  cancelEdit: () => void;
}

const ProductForm = ({ product, cancelEdit }: Props) => {
  const { control, handleSubmit, reset, watch, formState: { isDirty, isSubmitting } } = useForm({
    mode: "all",
    resolver: yupResolver(validationSchema)
  });

  const { brands, types } = useProducts();
  const watchFile = watch("file", null);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (product && !watchFile && !isDirty) {
      reset(product);
    }

    return () => {
      if (watchFile) {
        URL.revokeObjectURL(watchFile.preview);
      }
    }
  }, [product, reset, watchFile, isDirty]);

  const handleSubmitData = async (data: FieldValues) => {
    try {
      let response: Product;

      response = product ? await agent.Admin.updateProduct(data) : await agent.Admin.createProduct(data);

      dispatch(setProduct(response));
      cancelEdit();
    }
    catch (error) {
      console.log(error);
    }
  }

  return (
    <Box component={Paper} sx={{ p: 4 }}>
      <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
        Product Details
      </Typography>
      <form onSubmit={handleSubmit(handleSubmitData)}>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={12}>
            <AppTextInput control={control} label='Product name' name='name' />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppSelectList control={control} label='Brand' name='brand' items={brands} />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppSelectList control={control} label='Type' name='type' items={types} />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppTextInput control={control} label='Price' name='price' type="number" />
          </Grid>
          <Grid item xs={12} sm={6}>
            <AppTextInput control={control} label='Quantity in Stock' name='quantityInStock' type="number" />
          </Grid>
          <Grid item xs={12}>
            <AppTextInput control={control} label='Description' name='description' multiline={true} rows={3} />
          </Grid>
          <Grid item xs={12}>
            <Box alignItems="center" display="flex" justifyContent="space-between">
              <AppDropZone control={control} name="file" />
              {
                watchFile ?
                  (<img src={watchFile.preview} alt="preview" style={{ maxHeight: 200 }} />) :
                  (<img src={product?.imageUrl} alt={product?.name} style={{ maxHeight: 200 }} />)
              }
            </Box>
          </Grid>
        </Grid>
        <Box display='flex' justifyContent='space-between' sx={{ mt: 3 }}>
          <Button color='inherit' variant='contained' onClick={cancelEdit}>Cancel</Button>
          <LoadingButton color='success' variant='contained' type="submit" loading={isSubmitting}>
            Submit
          </LoadingButton>
        </Box>
      </form>
    </Box>
  )
}

export default ProductForm;