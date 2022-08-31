import { UploadFile } from '@mui/icons-material';
import { FormControl, FormHelperText, Typography } from '@mui/material';
import { useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
import { useController, UseControllerProps } from 'react-hook-form';

interface Props extends UseControllerProps { }

const AppDropZone = (props: Props) => {
  const { field, fieldState } = useController({ ...props, defaultValue: null });

  const dzStyles = {
    alignItems: "center",
    border: "dashed 3px #eee",
    borderColor: "#eee",
    borderRadius: "5px",
    display: "flex",
    height: 200,
    paddingTop: "30px",
    width: 500
  }

  const dzActive = {
    borderColor: "green"
  }

  const onDrop = useCallback((acceptedFiles: any[]) => {
    acceptedFiles[0] = Object.assign(acceptedFiles[0], {
      preview: URL.createObjectURL(acceptedFiles[0])
    });

    field.onChange(acceptedFiles[0]);
  }, [field]);

  const { getInputProps, getRootProps, isDragActive } = useDropzone({ onDrop });

  return (
    <div {...getRootProps()}>
      <FormControl error={!!fieldState.error} style={isDragActive ? { ...dzStyles, ...dzActive } : dzStyles}>
        <input {...getInputProps()} />
        <UploadFile sx={{ fontSize: "100px" }} />
        <Typography variant='h4'>Drop image here</Typography>
        <FormHelperText>{fieldState.error?.message}</FormHelperText>
      </FormControl>
    </div>
  );
}

export default AppDropZone;