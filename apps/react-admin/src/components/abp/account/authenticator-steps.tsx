import { useState } from "react";
import { Card, Steps, Button, Form, Input, QRCode } from "antd";
import { useTranslation } from "react-i18next";

import { verifyAuthenticatorCodeApi } from "@/api/account/profile";
import { useMutation } from "@tanstack/react-query";
import type { AuthenticatorDto } from "#/account/profile";
import { useCopyToClipboard } from "@/hooks/event/use-copy-to-clipboard";
import { toast } from "sonner";

interface Props {
	authenticator: AuthenticatorDto;
	onDone: () => void;
}

const AuthenticatorSteps: React.FC<Props> = ({ authenticator, onDone }) => {
	const { t: $t } = useTranslation();
	const { copyFn } = useCopyToClipboard();
	const [form] = Form.useForm();
	const [currentStep, setCurrentStep] = useState(0);
	const [codeValidated, setCodeValidated] = useState(false);
	const [recoveryCodes, setRecoveryCodes] = useState<string[]>([]);

	const steps = [
		{ title: $t("AbpAccount.Authenticator") },
		{ title: $t("AbpAccount.ValidAuthenticator") },
		{ title: $t("AbpAccount.RecoveryCode") },
	];

	const { mutateAsync: verifyCode, isPending: isLoading } = useMutation({
		mutationFn: verifyAuthenticatorCodeApi,
		onSuccess: (data) => {
			setRecoveryCodes(data.recoveryCodes);
			setCodeValidated(true);
			handleNextStep();
		},
	});

	const handleCopy = async (text?: string) => {
		if (!text) return;
		await copyFn(text);
		toast.success($t("AbpUi.CopiedToTheClipboard"));
	};

	const handleValidCode = async () => {
		try {
			const values = await form.validateFields();
			await verifyCode(values);
		} catch (error) {
			console.error("Validation failed:", error);
		}
	};

	const handlePrevStep = () => setCurrentStep((prev) => prev - 1);
	const handleNextStep = () => setCurrentStep((prev) => prev + 1);

	const renderStepContent = () => {
		switch (currentStep) {
			case 0:
				return (
					<Card className="mt-4">
						<Card.Meta
							title={
								<div className="h-16 flex flex-col justify-center">
									<span className="text-lg font-normal">{$t("AbpAccount.Authenticator")}</span>
									<span className="text-sm font-light">{$t("AbpAccount.AuthenticatorDesc")}</span>
								</div>
							}
						/>
						<div className="mt-2 flex flex-row flex-wrap">
							<div className="basis-1/2">
								<Card title={$t("AbpAccount.Authenticator:UseQrCode")} className="min-h-[350px]">
									<div className="flex justify-center">
										<QRCode value={authenticator.authenticatorUri} />
									</div>
								</Card>
							</div>
							<div className="basis-1/2">
								<Card
									title={$t("AbpAccount.Authenticator:InputCode")}
									className="min-h-[350px]"
									extra={
										<Button type="primary" onClick={() => handleCopy(authenticator.sharedKey)}>
											{$t("AbpAccount.Authenticator:CopyToClipboard")}
										</Button>
									}
								>
									<div className="flex items-center justify-center rounded-lg bg-[#dac6c6]">
										<div className="m-4 text-xl font-bold text-blue-600">{authenticator.sharedKey}</div>
									</div>
								</Card>
							</div>
						</div>
					</Card>
				);

			case 1:
				return (
					<Card className="mt-4">
						<Card.Meta
							title={
								<div className="h-16 flex flex-col justify-center">
									<span className="text-lg font-normal">{$t("AbpAccount.ValidAuthenticator")}</span>
									<span className="text-sm font-light">{$t("AbpAccount.ValidAuthenticatorDesc")}</span>
								</div>
							}
						/>
						<div className="flex flex-row">
							<div className="basis-2/3">
								<Form form={form}>
									<Form.Item
										label={$t("AbpAccount.DisplayName:AuthenticatorCode")}
										name="authenticatorCode"
										rules={[{ required: true }]}
									>
										<Input />
									</Form.Item>
								</Form>
							</div>
							<div className="ml-4 basis-2/3">
								<Button loading={isLoading} type="primary" onClick={handleValidCode}>
									{$t("AbpAccount.Validation")}
								</Button>
							</div>
						</div>
					</Card>
				);

			case 2:
				return (
					<Card
						className="mt-4"
						title={
							<div className="h-16 flex flex-col justify-center">
								<span className="text-lg font-normal">{$t("AbpAccount.RecoveryCode")}</span>
								<span className="text-sm font-light">{$t("AbpAccount.RecoveryCodeDesc")}</span>
							</div>
						}
						extra={
							<Button type="primary" onClick={() => handleCopy(recoveryCodes.join("\r"))}>
								{$t("AbpAccount.Authenticator:CopyToClipboard")}
							</Button>
						}
					>
						<div className="flex flex-col items-center justify-center rounded-lg bg-[#dac6c6]">
							<div className="m-2 text-xl font-bold text-blue-600">{recoveryCodes.slice(0, 5).join("\r\n")}</div>
							<div className="m-2 text-xl font-bold text-blue-600">{recoveryCodes.slice(5).join("\r\n")}</div>
						</div>
					</Card>
				);
		}
	};

	return (
		<Card bordered={false}>
			<Steps current={currentStep} items={steps} />
			{renderStepContent()}
			<div className="flex flex-row justify-end gap-2 pr-2 mt-4">
				{currentStep > 0 && !codeValidated && (
					<Button onClick={handlePrevStep}>{$t("AbpAccount.Steps:PreStep")}</Button>
				)}
				{currentStep < 2 && (
					<Button disabled={currentStep === 1 && !codeValidated} type="primary" onClick={handleNextStep}>
						{$t("AbpAccount.Steps:NextStep")}
					</Button>
				)}
				{currentStep === 2 && (
					<Button type="primary" onClick={onDone}>
						{$t("AbpAccount.Steps:Done")}
					</Button>
				)}
			</div>
		</Card>
	);
};

export default AuthenticatorSteps;
